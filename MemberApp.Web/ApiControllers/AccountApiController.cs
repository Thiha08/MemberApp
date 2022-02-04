using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels;
using MemberApp.Web.ViewModels.Params;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [ApiController]
    [Route("api/account")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AccountApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMembershipService _membershipService;
        private readonly ITokenService _tokenService;
        private readonly IMemberRepository _memberRepository;
        private readonly ILoggingRepository _loggingRepository;
        private string _generatedToken = null;

        public AccountApiController(
            IConfiguration config,
            IMembershipService membershipService,
            ITokenService tokenService,
            IMemberRepository memberRepository,
            ILoggingRepository loggingRepository)
        {
            _config = config;
            _membershipService = membershipService;
            _tokenService = tokenService;
            _memberRepository = memberRepository;
            _loggingRepository = loggingRepository;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginParams loginParams)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;

            try
            {
                MembershipContext _memberContext = _membershipService.ValidateMember(loginParams.Username, loginParams.Password);

                if (_memberContext.Member != null)
                {
                    var _roles = _memberRepository.GetMemberRoles(loginParams.Username)
                        .Select(x => x.Name)
                        .ToArray();

                    _generatedToken = _tokenService.BuildToken(
                        _config["Jwt:Key"].ToString(), 
                        _config["Jwt:Issuer"].ToString(),
                        _memberContext.Member.Username,
                        _roles);

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

        [HttpPost("register")]
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
