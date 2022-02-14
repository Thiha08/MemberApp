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
        public async Task<IActionResult> Put(long id, MemeberUpdateRequest request)
        {
            try
            {
                if (id != request.Id)
                    throw new Exception("Member not found");

                Member member = await _memberRepository.GetSingleAsync(s => s.Id == id);

                if (member == null)
                    throw new Exception("Member not found");

                var memberProtection = new MemberProtection
                {
                    Id = member.Id,
                    MemberId = member.Id,
                    FullName = request.FullName,
                    ServiceStatus = request.ServiceStatus,
                    UserName = request.PhoneNumber,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Rank = request.Rank,
                    CurrentCity = request.CurrentCity,
                    CadetNumber = request.CadetNumber,
                    CadetBattalion = request.CadetBattalion,
                    BCNumber = request.BCNumber,
                    LastBattalion = request.LastBattalion,
                    CurrentJob = request.CurrentJob,
                    ProtectionStatus = ProtectionStatus.Pending
                };

                if (memberProtection.ServiceStatus == ServiceStatus.Resigned)
                {
                    memberProtection.ResignationDate = request.ActionDate;
                    memberProtection.ResignationReason = request.ActionReason;
                }
                else if(memberProtection.ServiceStatus == ServiceStatus.Retired)
                {
                    memberProtection.RetiredDate = request.ActionDate;
                    memberProtection.RetiredReason = request.ActionReason;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Dismissed)
                {
                    memberProtection.DismissedDate = request.ActionDate;
                    memberProtection.DismissedReason = request.ActionReason;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.CDM)
                {
                    memberProtection.CdmDate = request.ActionDate;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Absence)
                {
                    memberProtection.AbsenceStartedDate = request.ActionDate;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Casualty)
                {
                    memberProtection.DateOfDeath = request.ActionDate;
                    memberProtection.BeneficiaryCity = request.BeneficiaryCity;
                    memberProtection.BeneficiaryPhoneNumber = request.BeneficiaryPhoneNumber;
                }
                else if (memberProtection.ServiceStatus == ServiceStatus.Death)
                {
                    memberProtection.DateOfDeath = request.ActionDate;
                    memberProtection.ReasonOfDeath = request.ActionReason;
                    memberProtection.BeneficiaryCity = request.BeneficiaryCity;
                    memberProtection.BeneficiaryPhoneNumber = request.BeneficiaryPhoneNumber;
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
