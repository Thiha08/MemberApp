using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Model.Entities;
using MemberApp.Model.ResultModels;
using MemberApp.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRepository<Member> _memberRepository;

        public MembersApiController(IRepository<Member> memberRepository)
        {
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                Member member = await _memberRepository.GetSingleAsync(s => s.Id == id, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

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
                    ActionDate = member.ActionDate,
                    ActionReason = member.ActionReason,
                    BeneficiaryAddress = member.BeneficiaryAddress,
                    BeneficiaryPhoneNumber = member.BeneficiaryPhoneNumber
                };

                var result = Result<MemberManagementViewModel>.Ok(viewModel);
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
