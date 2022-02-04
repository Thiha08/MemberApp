using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IMemberRepository _memberRepository;
        private readonly ILoggingRepository _loggingRepository;

        public AccountController(
            IMembershipService membershipService,
            IMemberRepository memberRepository,
            ILoggingRepository loggingRepository)
        {
            _membershipService = membershipService;
            _memberRepository = memberRepository;
            _loggingRepository = loggingRepository;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;

            try
            {
                MembershipContext _memberContext = _membershipService.ValidateMember(user.Username, user.Password);

                if (_memberContext.Member != null)
                {
                    IEnumerable<Role> _roles = _memberRepository.GetMemberRoles(user.Username);
                    List<Claim> _claims = new List<Claim>();
                    foreach (Role role in _roles)
                    {
                        Claim _claim = new Claim(ClaimTypes.Role, role.Name, ClaimValueTypes.String, user.Username);
                        _claims.Add(_claim);
                    }

                    //await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    //    new ClaimsPrincipal(new ClaimsIdentity(_claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                    //    new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties { IsPersistent = user.RememberMe });

                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Authentication succeeded"
                    };
                }
                else
                {
                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Authentication failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _authenticationResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            _result = new ObjectResult(_authenticationResult);
            return _result;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //await HttpContext.Authentication.SignOutAsync("Cookies");
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();

                return BadRequest();
            }

        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromBody] RegistrationViewModel user)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _registrationResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    Member _member = _membershipService.CreateMember(user.Username, user.PhoneNumber, user.Password, new int[] { 1 });

                    if (_member != null)
                    {
                        _registrationResult = new GenericResult()
                        {
                            Succeeded = true,
                            Message = "Registration succeeded"
                        };
                    }
                }
                else
                {
                    _registrationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Invalid fields."
                    };
                }
            }
            catch (Exception ex)
            {
                _registrationResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            _result = new ObjectResult(_registrationResult);
            return _result;
        }
    }
}
