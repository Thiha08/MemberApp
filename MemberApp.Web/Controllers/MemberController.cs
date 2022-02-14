using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Services.Abstract;
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
                    LockingStatus = x.User.IsLocked ? "Locked" : "",
                    RegisterationStatus = x.User.IsConfirmedByAdmin ? "Confirmed by Admin" :
                        (x.User.PhoneNumberConfirmed ? "Pending Admin Confirmation" : "Pending Phone Number Confirmation")
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

                await _smsService.SendSMSAsync(user.PhoneNumber, $"Your password for member app is {password}.");

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

            Member member = await _memberRepository.GetSingleAsync(id);

            viewModel.MemberOverview = new MemberOverviewViewModel
            {
                Id = member.Id,
                BCNumber = member.BCNumber,
                FullName = member.FullName,
                LastBattalion = member.LastBattalion,
                PhoneNumber = member.User.PhoneNumber,
                CurrentCity = member.CurrentCity,
                LockingStatus = member.User.IsLocked ? "Locked" : "",
                RegisterationStatus = member.User.IsConfirmedByAdmin ? "Confirmed by Admin" :
                        (member.User.PhoneNumberConfirmed ? "Pending Admin Confirmation" : "Pending Phone Number Confirmation")
            };

            MemberProtection protection = await _memberProtectionRepository
                .AllIncluding(x => x.ProtectionDetails)
                .Where(x => x.Status)
                .OrderBy(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            viewModel.MemberProtection = new MemberProtectionViewModel
            {
                Id = protection.Id,
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

            return View(viewModel);
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
