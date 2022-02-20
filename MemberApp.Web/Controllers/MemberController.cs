using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Extensions;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Model.ViewModels;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISmsService _smsService;

        public MemberController(
            IRepository<AuditLog> loggingRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRepository<Member> memberRepository,
            ISmsService smsService)
            : base(loggingRepository)
        {
            _memberRepository = memberRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _smsService = smsService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<MemberOverviewViewModel> viewModel = await _memberRepository
                .AllIncluding(x => x.User)
                .Where(x => x.Status)
                .Select(x => new MemberOverviewViewModel
                {
                    Id = x.Id,
                    BCNumber = x.BCNumber,
                    FullName = x.FullName,
                    Rank = x.Rank,
                    CurrentStatus = x.ServiceStatus,
                    Division = x.Division,
                    PhoneNumber = x.User.PhoneNumber,
                    Request = ""
                })
                .ToListAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles
                .Where(x => x.Name != "SuperAdmin")
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MemberCreateViewModel viewModel)
        {
            try
            {
                ViewBag.Roles = await _roleManager.Roles
                    .Where(x => x.Name != "SuperAdmin")
                    .ToListAsync();

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
                    Email = viewModel.Email,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsConfirmedByAdmin = true,
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
                    FullName = viewModel.FullName,
                    ServiceStatus = viewModel.ServiceStatus,
                    PermanentContactNumber = viewModel.PermanentContactNumber,
                    Address = viewModel.Address,
                    Job = viewModel.Job,
                    CadetNumber = viewModel.CadetNumber,
                    CadetBattalion = viewModel.CadetBattalion,
                    Rank = viewModel.Rank,
                    BCNumber = viewModel.BCNumber,
                    Battalion = viewModel.Battalion,
                    Division = viewModel.Division,
                    ActionDate = viewModel.ActionDate?.ToUniversalTime(),
                    ActionReason = viewModel.ActionReason,
                    BeneficiaryAddress = viewModel.BeneficiaryAddress,
                    BeneficiaryPhoneNumber = viewModel.BeneficiaryPhoneNumber,
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
        public async Task<ActionResult> Edit(long id)
        {
            ViewBag.Roles = await _roleManager.Roles
                .Where(x => x.Name != "SuperAdmin")
                .ToListAsync();

            Member member = await _memberRepository.GetSingleAsync(x => x.Id == id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            var userRoles = await _userManager.GetRolesAsync(member.User);

            var role = await _roleManager.FindByNameAsync(userRoles.FirstOrDefault());

            var viewModel = new MemberEditViewModel
            {
                Id = member.Id,
                FullName = member.FullName,
                PhoneNumber = member.User.PhoneNumber,
                PermanentContactNumber = member.PermanentContactNumber,
                Email = member.User.Email,
                Role = role.Id,
                ServiceStatus = member.ServiceStatus,
                Address = member.Address,
                Job = member.Job,
                CadetNumber = member.CadetNumber,
                CadetBattalion = member.CadetBattalion,
                Rank = member.Rank,
                BCNumber = member.BCNumber,
                Battalion = member.Battalion,
                Division = member.Division,
                ActionDate = member.ActionDate,
                ActionReason = member.ActionReason,
                BeneficiaryAddress = member.BeneficiaryAddress,
                BeneficiaryPhoneNumber = member.BeneficiaryPhoneNumber
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MemberEditViewModel viewModel)
        {
            try
            {
                ViewBag.Roles = await _roleManager.Roles
                    .Where(x => x.Name != "SuperAdmin")
                    .ToListAsync();

                if (!ModelState.IsValid)
                    return View(viewModel);

                Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

                if (member == null)
                {
                    GenerateAlertMessage(false, "Member not found");
                    return RedirectToAction(nameof(Index));
                }

                member.User.UserName = viewModel.PhoneNumber;
                member.User.PhoneNumber = viewModel.PhoneNumber;
                member.User.Email = viewModel.Email;
                member.User.UpdatedDate = DateTime.UtcNow;

                var userRoles = await _userManager.GetRolesAsync(member.User);

                var oldRole = await _roleManager.FindByNameAsync(userRoles.FirstOrDefault());

                var newRole = await _roleManager.FindByIdAsync(viewModel.Role);

                if (newRole != null && oldRole.Id != newRole.Id)
                {
                    await _userManager.RemoveFromRoleAsync(member.User, oldRole.Name);
                    await _userManager.AddToRoleAsync(member.User, newRole.Name);
                }

                member.FullName = viewModel.FullName;
                member.ServiceStatus = viewModel.ServiceStatus;
                member.PermanentContactNumber = viewModel.PermanentContactNumber;
                member.Address = viewModel.Address;
                member.Job = viewModel.Job;
                member.CadetNumber = viewModel.CadetNumber;
                member.CadetBattalion = viewModel.CadetBattalion;
                member.Rank = viewModel.Rank;
                member.BCNumber = viewModel.BCNumber;
                member.Battalion = viewModel.Battalion;
                member.Division = viewModel.Division;
                member.ActionDate = viewModel.ActionDate;
                member.ActionReason = viewModel.ActionReason;
                member.BeneficiaryAddress = viewModel.BeneficiaryAddress;
                member.BeneficiaryPhoneNumber = viewModel.BeneficiaryPhoneNumber;

                await _memberRepository.UpdateAsync(member);
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
            Member member = await _memberRepository.GetSingleAsync(x => x.Id == id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new MemberManagementViewModel
            {
                Id = member.Id,
                FullName = member.FullName,
                PhoneNumber = member.User.PhoneNumber,
                PermanentContactNumber = member.PermanentContactNumber,
                Email = member.User.Email,
                ServiceStatus = member.ServiceStatus,
                Address = member.Address,
                Job = member.Job,
                CadetNumber = member.CadetNumber,
                CadetBattalion = member.CadetBattalion,
                Rank = member.Rank,
                BCNumber = member.BCNumber,
                Battalion = member.Battalion,
                Division = member.Division,
                ActionDate = member.ActionDate,
                ActionReason = member.ActionReason,
                BeneficiaryAddress = member.BeneficiaryAddress,
                BeneficiaryPhoneNumber = member.BeneficiaryPhoneNumber,
                IsLocked = member.User.IsLocked,
                IsConfirmedByAdmin = member.User.IsConfirmedByAdmin,
                PermissionStatus = member.PermissionStatus,
                PermissionDate = member.PermissionDate,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsConfirmedByAdmin = true;
            member.User.UpdatedDate = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unconfirm(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsConfirmedByAdmin = false;
            member.User.UpdatedDate = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Lock(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsLocked = true;
            member.User.UpdatedDate = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member locked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unlock(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.User.IsLocked = false;
            member.User.UpdatedDate = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member unlocked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApprovePermissionToEdit(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.PermissionStatus = PermissionStatus.Approved;
            member.PermissionDate = DateTime.UtcNow;
            member.EditOTP = Constants.GenerateOTPCode;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member locked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectPermissionToEdit(MemberIdViewModel viewModel)
        {
            Member member = await _memberRepository.GetSingleAsync(x => x.Status && x.Id == viewModel.Id, x => x.User);

            if (member == null)
            {
                GenerateAlertMessage(false, "Member not found");
                return RedirectToAction(nameof(Index));
            }

            member.PermissionStatus = PermissionStatus.Rejected;
            member.PermissionDate = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.CommitAsync();

            GenerateAlertMessage(true, "Member unlocked");
            return RedirectToAction(nameof(Manage), new { id = viewModel.Id });
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
