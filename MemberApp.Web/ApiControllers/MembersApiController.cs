using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Model.RequestModels;
using MemberApp.Model.ResultModels;
using MemberApp.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MembersApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Member> _memberRepository;

        public MembersApiController(UserManager<ApplicationUser> userManager, IRepository<Member> memberRepository)
        {
            _userManager = userManager;
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string keywords)
        {
            try
            {
                IQueryable<Member> query = _memberRepository.GetAll()
                    .Where(x => x.Status);

                if (!string.IsNullOrWhiteSpace(keywords))
                    query = query.Where(x => x.FullName.Contains(keywords));

                var membersVM = await query.Select(
                    x => new MemberOverviewResult
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        ServiceStatus = x.ServiceStatus
                    })
                    .OrderBy(x => x.FullName)
                    .ToListAsync();

                var result = Result<List<MemberOverviewResult>>.Ok(membersVM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                Member member = await _memberRepository.GetSingleAsync(s => s.Id == id, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

                var viewModel = new MemberDetailsViewModel
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
                };

                var result = Result<MemberDetailsViewModel>.Ok(viewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                var exists = await _userManager.FindByNameAsync(userName);

                if (exists == null)
                    throw new Exception("Please login again");

                Member member = await _memberRepository.GetSingleAsync(x => x.ApplicationUserId == exists.Id, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

                var viewModel = new MemberProfileViewModel
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
                };

                if (member.PermissionStatus == PermissionStatus.Approved && member.PermissionDate.Value.AddDays(1) >= DateTime.UtcNow)
                {
                    viewModel.EditOTP = member.EditOTP;
                }

                var result = Result<MemberProfileViewModel>.Ok(viewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] MemberUpdateData data)
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                var exists = await _userManager.FindByNameAsync(userName);

                if (exists == null)
                    throw new Exception("Please login again");

                Member member = await _memberRepository.GetSingleAsync(x => x.ApplicationUserId == exists.Id, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

                if (member.PermissionStatus == PermissionStatus.Pending)
                    throw new Exception("Edit request approval is still pending");

                if (member.PermissionStatus == PermissionStatus.Rejected)
                    throw new Exception("Edit request approval is rejected");

                if (member.PermissionStatus != PermissionStatus.Approved || member.PermissionDate.Value.AddDays(1) < DateTime.UtcNow)
                    throw new Exception("Edit request approval is expired");

                if (member.EditOTP != data.EditOTP)
                    throw new Exception("Invalid edit OTP");

                member.User.UserName = data.PhoneNumber;
                member.User.PhoneNumber = data.PhoneNumber;
                member.User.Email = data.Email;
                member.User.UpdatedDate = DateTime.UtcNow;
                member.FullName = data.FullName;
                member.ServiceStatus = data.ServiceStatus;
                member.PermanentContactNumber = data.PermanentContactNumber;
                member.Address = data.Address;
                member.Job = data.Job;
                member.CadetNumber = data.CadetNumber;
                member.CadetBattalion = data.CadetBattalion;
                member.Rank = data.Rank;
                member.BCNumber = data.BCNumber;
                member.Battalion = data.Battalion;
                member.Division = data.Division;
                member.ActionDate = data.ActionDate;
                member.ActionReason = data.ActionReason;
                member.BeneficiaryAddress = data.BeneficiaryAddress;
                member.BeneficiaryPhoneNumber = data.BeneficiaryPhoneNumber;

                member.PermissionStatus = PermissionStatus.Default;
                member.PermissionDate = DateTime.UtcNow;
                member.EditOTP = null;

                await _memberRepository.UpdateAsync(member);
                await _memberRepository.CommitAsync();

                // await _smsService.SendSMSAsync(user.PhoneNumber, $"Your password for member app is {password}.");

                var result = Result.Ok("Member updated successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPost("requestToEdit")]
        public async Task<IActionResult>RequestToEdit()
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                var exists = await _userManager.FindByNameAsync(userName);

                if (exists == null)
                    throw new Exception("Please login again");

                Member member = await _memberRepository.GetSingleAsync(x => x.ApplicationUserId == exists.Id, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

                member.PermissionStatus = PermissionStatus.Pending;
                member.PermissionDate = DateTime.UtcNow;

                await _memberRepository.UpdateAsync(member);
                await _memberRepository.CommitAsync();

                var result = Result.Ok("Requested successfully");
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
