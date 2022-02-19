using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Extensions;
using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.DTOs;
using MemberApp.Web.ViewModels.RequestDTOs;
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
                    x => new MemberOverviewDTO
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        ServiceStatus = x.ServiceStatus
                    })
                    .OrderBy(x => x.FullName)
                    .ToListAsync();

                var result = Result<List<MemberOverviewDTO>>.Ok(membersVM);
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

                var memberDTO = new MemberDetailsDTO
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
                    Battalion = member.Battalion
                };

                if (member.ServiceStatus == ServiceStatus.Retired)
                {
                    memberDTO.ActionDate = member.RetiredDate;
                    memberDTO.ActionReason = member.RetiredReason;
                }
                else if (member.ServiceStatus == ServiceStatus.Resigned)
                {
                    memberDTO.ActionDate = member.ResignationDate;
                    memberDTO.ActionReason = member.ResignationReason;
                }
                else if (member.ServiceStatus == ServiceStatus.Dismissed)
                {
                    memberDTO.ActionDate = member.DismissedDate;
                    memberDTO.ActionReason = member.DismissedReason;
                }
                else if (member.ServiceStatus == ServiceStatus.Absence)
                {
                    memberDTO.ActionDate = member.AbsenceStartedDate;
                }
                else if (member.ServiceStatus == ServiceStatus.CDM)
                {
                    memberDTO.ActionDate = member.CdmDate;
                }
                else if (member.ServiceStatus == ServiceStatus.Casualty)
                {
                    memberDTO.ActionDate = member.DateOfDeath;
                    memberDTO.BeneficiaryAddress = member.BeneficiaryAddress;
                    memberDTO.BeneficiaryPhoneNumber = member.BeneficiaryPhoneNumber;
                }
                else if (member.ServiceStatus == ServiceStatus.Death)
                {
                    memberDTO.ActionDate = member.DateOfDeath;
                    memberDTO.ActionReason = member.ReasonOfDeath;
                    memberDTO.BeneficiaryAddress = member.BeneficiaryAddress;
                    memberDTO.BeneficiaryPhoneNumber = member.BeneficiaryPhoneNumber;
                }

                var result = Result<MemberDetailsDTO>.Ok(memberDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(MemeberUpdateRequest request)
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                Member member = await _memberRepository.GetSingleAsync(s => s.User.UserName == userName, x => x.User);

                if (member == null)
                    throw new Exception("Member not found");

                var result = Result.Ok();
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
