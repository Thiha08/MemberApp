using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.DTOs;
using MemberApp.Web.ViewModels.RequestDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly ISmsService _smsService;
       
        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ITokenService tokenService,
            ISmsService smsService)
        {
            _userManager = userManager;
            _config = config;
            _tokenService = tokenService;
            _smsService = smsService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.PhoneNumber);

                if (user == null)
                    throw new Exception("Phone number or password is wrong");

                bool isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

                if(!isValidPassword)
                    throw new Exception("Phone number or password is wrong");

                var roles = await _userManager.GetRolesAsync(user);

                var generatedToken = _tokenService.BuildToken(
                    _config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(),
                    user.UserName,
                    roles.ToArray());

                var result = Result<LoginDTO>.Ok(new LoginDTO
                {
                    AccessToken = generatedToken
                }, "Authentication succeeded");

                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPost("loginWithOTP")]
        public async Task<IActionResult> LoginWithOTP([FromBody] LoginWithOTPRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.PhoneNumber);

                if (user == null)
                    throw new Exception("Phone number or password is wrong");

                bool isValidOTP = CheckLoginOTPCode(user, request.OTPCode);

                if (!isValidOTP)
                    throw new Exception("Invalid OTP");

                var roles = await _userManager.GetRolesAsync(user);

                var generatedToken = _tokenService.BuildToken(
                    _config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(),
                    user.UserName,
                    roles.ToArray());

                var result = Result<LoginDTO>.Ok(new LoginDTO
                {
                    AccessToken = generatedToken
                }, "Authentication succeeded");

                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPost("sendOTP")]
        public async Task<IActionResult> SendOTP([FromBody] OTPCodeRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.PhoneNumber);

                if (user == null)
                    throw new Exception("Phone number or password is wrong.");

                user.OTPCode = Constants.GenerateOTPCode;
                user.OTPCodeExpiryDate = DateTime.UtcNow.AddSeconds(Constants.OTPCodeExpirySeconds);

                await _userManager.UpdateAsync(user);
                await _smsService.SendSMSAsync(user.PhoneNumber, $"MemberApp: Your OTP code is : {user.OTPCode}");

                var result = Result.Ok($"OTP code sent to {user.PhoneNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
            
        }

        private bool CheckLoginOTPCode(ApplicationUser user, string otpCode)
        {
            return user.OTPCode == otpCode && user.OTPCodeExpiryDate >= DateTime.UtcNow;
        }
    }
}
