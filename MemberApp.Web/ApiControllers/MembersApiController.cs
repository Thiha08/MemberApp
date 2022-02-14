using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Extensions;
using MemberApp.Data.Infrastructure.Core.Result;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Constants;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.DTOs;
using MemberApp.Web.ViewModels.Params;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MembersApiController : ControllerBase
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<MemberProtection> _memberProtectionRepository;

        public MembersApiController(IRepository<Member> memberRepository, IRepository<MemberProtection> memberProtectionRepository)
        {
            _memberRepository = memberRepository;
            _memberProtectionRepository = memberProtectionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string keywords)
        {
            try
            {
                IQueryable<Member> query = _memberRepository.GetAll()
                    .Where(x => x.Status);

                if (string.IsNullOrWhiteSpace(keywords))
                    query = query.Where(x => x.FullName.Contains(keywords));

                var membersVM = await query.Select(
                    x => new MemberOverviewDTO
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        ServiceStatus = x.ServiceStatus.ToDescription()
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
                    ServiceStatus = member.ServiceStatus.ToDescription(),
                    PhoneNumber = member.User.PhoneNumber,
                    Email = member.User.Email,
                    Rank = member.Rank,
                    CurrentCity = member.CurrentCity,
                    CadetNumber = member.CadetNumber,
                    CadetBattalion = member.CadetBattalion,
                    BCNumber = member.BCNumber,
                    LastBattalion = member.LastBattalion,
                    CurrentJob = member.CurrentJob,
                };

                var result = Result<MemberDetailsDTO>.Ok(memberDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = Result.BadRequest(ex.Message);
                return Ok(result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, MemeberUpdateParams updateParams)
        {
            try
            {
                if (id != updateParams.Id)
                    throw new Exception("Member not found");

                Member member = await _memberRepository.GetSingleAsync(s => s.Id == id);

                if (member == null)
                    throw new Exception("Member not found");

                var memberProtection = new MemberProtection
                {
                    Id = member.Id,
                    MemberId = member.Id,
                    FullName = updateParams.FullName,
                    ServiceStatus = updateParams.ServiceStatus,
                    UserName = updateParams.PhoneNumber,
                    PhoneNumber = updateParams.PhoneNumber,
                    Email = updateParams.Email,
                    Rank = updateParams.Rank,
                    CurrentCity = updateParams.CurrentCity,
                    CadetNumber = updateParams.CadetNumber,
                    CadetBattalion = updateParams.CadetBattalion,
                    BCNumber = updateParams.BCNumber,
                    LastBattalion = updateParams.LastBattalion,
                    CurrentJob = updateParams.CurrentJob,
                    ProtectionStatus = ProtectionStatus.Pending
                };

                if (memberProtection.ServiceStatus == ServiceStatus.Resigned)
                {
                    memberProtection.ResignationDate = updateParams.ActionDate;
                    memberProtection.ResignationReason = updateParams.ActionReason;
                }
                else if(memberProtection.ServiceStatus == ServiceStatus.Retired)
                {
                    memberProtection.RetiredDate = updateParams.ActionDate;
                    memberProtection.RetiredReason = updateParams.ActionReason;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Dismissed)
                {
                    memberProtection.DismissedDate = updateParams.ActionDate;
                    memberProtection.DismissedReason = updateParams.ActionReason;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.CDM)
                {
                    memberProtection.CdmDate = updateParams.ActionDate;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Absence)
                {
                    memberProtection.AbsenceStartedDate = updateParams.ActionDate;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Casualty)
                {
                    memberProtection.DateOfDeath = updateParams.ActionDate;
                    memberProtection.BeneficiaryCity = updateParams.BeneficiaryCity;
                    memberProtection.BeneficiaryPhoneNumber = updateParams.BeneficiaryPhoneNumber;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Death)
                {
                    memberProtection.DateOfDeath = updateParams.ActionDate;
                    memberProtection.ReasonOfDeath = updateParams.ActionReason;
                    memberProtection.BeneficiaryCity = updateParams.BeneficiaryCity;
                    memberProtection.BeneficiaryPhoneNumber = updateParams.BeneficiaryPhoneNumber;
                }

                await _memberProtectionRepository.UpdateAsync(memberProtection);
                await _memberProtectionRepository.CommitAsync();

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
