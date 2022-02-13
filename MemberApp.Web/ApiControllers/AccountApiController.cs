using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.DTOs;
using MemberApp.Web.ViewModels.Params;
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
       
        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginParams loginParams)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginParams.PhoneNumber);

                if (user == null)
                    throw new Exception("Phone number or password is wrong.");

                bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginParams.Password);

                if(!isValidPassword)
                    throw new Exception("Phone number or password is wrong.");

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
    }
}
