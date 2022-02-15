using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MemberApp.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IRepository<AuditLog> loggingRepository,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
            : base(loggingRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Member");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                await _loggingRepository.AddAsync(new AuditLog() { LogLevel = LogLevel.Error, Message = ex.Message, StackTrace = ex.StackTrace });
                await _loggingRepository.CommitAsync();

                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                await _loggingRepository.AddAsync(new AuditLog() { LogLevel = LogLevel.Error, Message = ex.Message, StackTrace = ex.StackTrace });
                await _loggingRepository.CommitAsync();

                return RedirectToAction(nameof(Login));
            }

        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
