using MemberApp.Data.Abstract;
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
using System.Text;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Member> _memberRepository;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly ISmsService _smsService;
       
        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            IRepository<Member> memberRepository,
            IConfiguration config,
            ITokenService tokenService,
            ISmsService smsService)
        {
            _userManager = userManager;
            _memberRepository = memberRepository;
            _config = config;
            _tokenService = tokenService;
            _smsService = smsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            try
            {
                var exists = await _userManager.FindByNameAsync(request.PhoneNumber);

                if (exists != null)
                    throw new Exception("The phone number already exists");

                var user = new ApplicationUser
                {
                    UserName = request.PhoneNumber,
                    PhoneNumber = request.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsConfirmedByAdmin = false,
                    Status = true
                };

                string password = GeneratePassword();
                var userResult = await _userManager.CreateAsync(user, password);

                if (!userResult.Succeeded)
                    throw new Exception("User creation failed, please contact admin");

                await _userManager.AddToRoleAsync(user, "User");

                var member = new Member
                {
                    ApplicationUserId = user.Id,
                };

                await _memberRepository.AddAsync(member);
                await _memberRepository.CommitAsync();

                // await _smsService.SendSMSAsync(user.PhoneNumber, $"Your password for member app is {password}.");

                var result = Result.Ok("Registered successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.PhoneNumber);

                if (user == null)
                    throw new Exception("Phone number or password is wrong");

                if(!user.IsConfirmedByAdmin)
                    throw new Exception("Please contact admin to confirm your account");

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

                if (!user.IsConfirmedByAdmin)
                    throw new Exception("Please contact admin to confirm your account");

                bool isValidOTP = CheckLoginOTPCode(user, request.OTPCode);

                if (!isValidOTP)
                    throw new Exception("Invalid OTP");

                var roles = await _userManager.GetRolesAsync(user);

                var generatedToken = _tokenService.BuildToken(
                    _config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(),
                    user.UserName,
                    roles.ToArray());

                if (!user.PhoneNumberConfirmed)
                    user.PhoneNumberConfirmed = true;

                user.OTPCodeExpiryDate = DateTime.UtcNow; // release OTP code
                await _userManager.UpdateAsync(user);

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
