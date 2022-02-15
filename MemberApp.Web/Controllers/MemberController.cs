using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Extensions;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberApp.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class MemberController : BaseController
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<MemberProtection> _memberProtectionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISmsService _smsService;

        public MemberController(
            IRepository<AuditLog> loggingRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRepository<Member> memberRepository,
            IRepository<MemberProtection> memberProtectionRepository,
            ISmsService smsService)
            : base(loggingRepository)
        {
            _memberRepository = memberRepository;
            _memberProtectionRepository = memberProtectionRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _smsService = smsService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<MemberOverviewViewModel> viewModel = await _memberRepository
                .AllIncluding(x => x.User)
                .Select(x => new MemberOverviewViewModel
                {
                    Id = x.Id,
                    BCNumber = x.BCNumber,
                    FullName = x.FullName,
                    LastBattalion = x.LastBattalion,
                    PhoneNumber = x.User.PhoneNumber,
                    CurrentCity = x.CurrentCity,
                    IsLocked = x.User.IsLocked,
                    IsConfirmedByAdmin = x.User.IsConfirmedByAdmin
                })
                .ToListAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MemberCreateViewModel viewModel)
        {
            try
            {
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();

                if (!ModelState.IsValid)
                    return View(viewModel);

                var exists = await _userManager.FindByNameAsync(viewModel.PhoneNumber);

                if (exists != null)
                {
                    GenerateAlertMessage(false, "The phone number already exists");
                    return View(viewModel);
                }

                var user = new ApplicationUser
                {
                    UserName = viewModel.PhoneNumber,
                    PhoneNumber = viewModel.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = viewModel.Email,
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Status = true
                };

                string password = GeneratePassword();
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    GenerateAlertMessage(false, "The member is failed to create");

                    return View(viewModel);
                }

                var role = await _roleManager.FindByIdAsync(viewModel.Role);

                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                var member = new Member
                {
                    ApplicationUserId = user.Id,
                    FullName = viewModel.FullName
                };

                await _memberRepository.AddAsync(member);
                await _memberRepository.CommitAsync();

                // await _smsService.SendSMSAsync(user.PhoneNumber, $"Your password for member app is {password}.");

                GenerateAlertMessage(true, "The member is created successfully.");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                GenerateAlertMessage(false, ex.Message);
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Manage(long id)
        {
            var viewModel = new MemberManagementViewModel();

            Member member = await _memberRepository.GetSingleAsync(x => x.Id == id, x => x.User);

            viewModel.MemberOverview = new MemberOverviewViewModel
            {
                Id = member.Id,
                BCNumber = member.BCNumber,
                FullName = member.FullName,
                LastBattalion = member.LastBattalion,
                PhoneNumber = member.User.PhoneNumber,
                CurrentCity = member.CurrentCity,
                IsLocked = member.User.IsLocked,
                IsConfirmedByAdmin = member.User.IsConfirmedByAdmin
            };

            viewModel.MemberProtection = new MemberProtectionViewModel();

            MemberProtection protection = await _memberProtectionRepository
                .AllIncluding(x => x.ProtectionDetails)
                .Where(x => x.Status && x.MemberId == member.Id)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            if (protection != null)
            {
                viewModel.MemberProtection = new MemberProtectionViewModel
                {
                    Id = protection.Id,
                    MemberId = protection.MemberId,
                    Status = protection.ProtectionStatus,
                    MemberProtectionDetails = protection.ProtectionDetails
                    .Select(x => new MemberProtectionDetailViewModel
                    {
                        Id = x.Id,
                        KeyName = x.KeyName,
                        OldValue = x.OldValue,
                        NewValue = x.NewValue,
                        Status = x.ProtectionStatus
                    })
                    .ToList()
                };
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmMemberViewModel viewModel)
        {
            var member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsConfirmedByAdmin = true;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Lock(LockMemberViewModel viewModel)
        {
            var member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsLocked = true;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member locked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unlock(UnlockMemberViewModel viewModel)
        {
            var member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsLocked = false;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member unlocked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveAllChanges(ApproveAllChangesViewModel viewModel)
        {
            Member member = await _memberRepository
                .GetSingleAsync(x => x.Id == viewModel.MemberId, x => x.User);

            MemberProtection memberProtection = await _memberProtectionRepository
                .GetSingleAsync(x => x.Id == viewModel.ProtectionId, x => x.ProtectionDetails);

            if(member == null || memberProtection == null || memberProtection.ProtectionDetails == null)
            {
                GenerateAlertMessage(true, "Failed to update changes");
                return RedirectToAction(nameof(Manage), new { id = viewModel.MemberId });
            }

            List<MemberProtectionDetail> changes = memberProtection.ProtectionDetails
                .Where(x => x.ProtectionStatus == ProtectionStatus.Pending && x.NewValue != x.OldValue)
                .ToList();

            var memberViewModel = new MemberViewModel();

            foreach (var change in changes)
            {
                ReflectionExtensions.TrySetProtectionValue(memberViewModel, change.KeyName, change.NewValue);
            }

            if (!string.IsNullOrWhiteSpace(memberViewModel.FullName))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.FullName));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.FullName = memberViewModel.FullName != Constants.DeletedProperty ? memberViewModel.FullName : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.CurrentCity))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.CurrentCity));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.CurrentCity = memberViewModel.CurrentCity != Constants.DeletedProperty ? memberViewModel.CurrentCity : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.CurrentJob))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.CurrentJob));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.CurrentJob = memberViewModel.CurrentJob != Constants.DeletedProperty ? memberViewModel.CurrentJob : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.CadetNumber))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.CadetNumber));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.CadetNumber = memberViewModel.CadetNumber != Constants.DeletedProperty ? memberViewModel.CadetNumber : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.CadetBattalion))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.CadetBattalion));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.CadetBattalion = memberViewModel.CadetBattalion != Constants.DeletedProperty ? memberViewModel.CadetBattalion : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.Rank))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.Rank));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.Rank = memberViewModel.Rank != Constants.DeletedProperty ? memberViewModel.Rank : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.BCNumber))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.BCNumber));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.BCNumber = memberViewModel.BCNumber != Constants.DeletedProperty ? memberViewModel.BCNumber : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.LastBattalion))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.LastBattalion));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.LastBattalion = memberViewModel.LastBattalion != Constants.DeletedProperty ? memberViewModel.LastBattalion : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.ResignationDate))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.ResignationDate));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.ResignationDate = memberViewModel.ResignationDate != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.ResignationDate) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.ResignationReason))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.ResignationReason));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.ResignationReason = memberViewModel.ResignationReason != Constants.DeletedProperty ? memberViewModel.ResignationReason : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.RetiredDate))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.RetiredDate));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.RetiredDate = memberViewModel.RetiredDate != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.RetiredDate) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.RetiredReason))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.RetiredReason));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.RetiredReason = memberViewModel.RetiredReason != Constants.DeletedProperty ? memberViewModel.RetiredReason : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.DismissedDate))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.DismissedDate));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.DismissedDate = memberViewModel.DismissedDate != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.DismissedDate) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.DismissedReason))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.DismissedReason));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.DismissedReason = memberViewModel.DismissedReason != Constants.DeletedProperty ? memberViewModel.DismissedReason : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.CdmDate))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.CdmDate));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.CdmDate = memberViewModel.CdmDate != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.CdmDate) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.AbsenceStartedDate))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.AbsenceStartedDate));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.AbsenceStartedDate = memberViewModel.AbsenceStartedDate != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.AbsenceStartedDate) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.DateOfDeath))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.DateOfDeath));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.DateOfDeath = memberViewModel.DateOfDeath != Constants.DeletedProperty ? DateTime.Parse(memberViewModel.DateOfDeath) : null;
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.ReasonOfDeath))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.ReasonOfDeath));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.ReasonOfDeath = memberViewModel.ReasonOfDeath != Constants.DeletedProperty ? memberViewModel.ReasonOfDeath : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.BeneficiaryCity))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.BeneficiaryCity));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.BeneficiaryCity = memberViewModel.BeneficiaryCity != Constants.DeletedProperty ? memberViewModel.BeneficiaryCity : "";
            }
            if (!string.IsNullOrWhiteSpace(memberViewModel.BeneficiaryPhoneNumber))
            {
                var protection = changes.FirstOrDefault(x => x.KeyName == nameof(member.BeneficiaryPhoneNumber));
                protection.ProtectionStatus = ProtectionStatus.Approved;

                member.BeneficiaryPhoneNumber = memberViewModel.BeneficiaryPhoneNumber != Constants.DeletedProperty ? memberViewModel.BeneficiaryPhoneNumber : "";
            }

            await _memberProtectionRepository.UpdateAsync(memberProtection);
            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Approved all changes successfully");
            return RedirectToAction(nameof(Manage), new { id = viewModel.MemberId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(ApproveViewModel viewModel)
        {
            GenerateAlertMessage(true, "Approved successfully");
            return RedirectToAction(nameof(Manage), new { id = viewModel.MemberId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(RejectViewModel viewModel)
        {
            GenerateAlertMessage(true, "Rejected successfully");
            return RedirectToAction(nameof(Manage), new { id = viewModel.MemberId });
        }



        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;
            bool nonAlphanumeric = options.RequireNonAlphanumeric;

            var password = new StringBuilder();
            var random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
    }
}
