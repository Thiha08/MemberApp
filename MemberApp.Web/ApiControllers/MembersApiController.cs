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
