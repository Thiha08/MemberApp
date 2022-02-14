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
using System.Reflection;
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
                    MemberId = member.Id,
                    ProtectionDetails = new List<MemberProtectionDetail>()
                };

                if (member.User.UserName != request.PhoneNumber)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.User.UserName),
                        OldValue = member.User.UserName,
                        NewValue = request.PhoneNumber
                    });
                }
                if (member.User.PhoneNumber != request.PhoneNumber)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.User.PhoneNumber),
                        OldValue = member.User.PhoneNumber,
                        NewValue = request.PhoneNumber
                    });
                }
                if (member.User.Email != request.Email)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.User.Email),
                        OldValue = member.User.Email,
                        NewValue = request.Email
                    });
                }
                if (member.FullName != request.FullName)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.FullName),
                        OldValue = member.FullName,
                        NewValue = request.FullName
                    });
                }
                if (member.ServiceStatus != request.ServiceStatus)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.ServiceStatus),
                        OldValue = Convert.ToString(member.ServiceStatus),
                        NewValue = Convert.ToString(request.ServiceStatus)
                    });
                }
                if (member.Rank != request.Rank)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.Rank),
                        OldValue = member.Rank,
                        NewValue = request.Rank
                    });
                }
                if (member.CurrentCity != request.CurrentCity)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.CurrentCity),
                        OldValue = member.CurrentCity,
                        NewValue = request.CurrentCity
                    });
                }
                if (member.CadetNumber != request.CadetNumber)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.CadetNumber),
                        OldValue = member.CadetNumber,
                        NewValue = request.CadetNumber
                    });
                }
                if (member.CadetBattalion != request.CadetBattalion)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.CadetBattalion),
                        OldValue = member.CadetBattalion,
                        NewValue = request.CadetBattalion
                    });
                }
                if (member.BCNumber != request.BCNumber)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.BCNumber),
                        OldValue = member.BCNumber,
                        NewValue = request.BCNumber
                    });
                }
                if (member.LastBattalion != request.LastBattalion)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.LastBattalion),
                        OldValue = member.LastBattalion,
                        NewValue = request.LastBattalion
                    });
                }
                if (member.CurrentJob != request.CurrentJob)
                {
                    memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                    {
                        KeyName = nameof(member.CurrentJob),
                        OldValue = member.CurrentJob,
                        NewValue = request.CurrentJob
                    });
                }

                if (request.ServiceStatus == ServiceStatus.Resigned)
                {
                    if (member.ResignationDate != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.ResignationDate),
                            OldValue = member.ResignationDate?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                    if (member.ResignationReason != request.ActionReason)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.ResignationReason),
                            OldValue = member.ResignationReason,
                            NewValue = request.ActionReason
                        });
                    }
                }
                else if(request.ServiceStatus == ServiceStatus.Retired)
                {
                    if (member.RetiredDate != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.RetiredDate),
                            OldValue = member.RetiredDate?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                    if (member.RetiredReason != request.ActionReason)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.RetiredReason),
                            OldValue = member.RetiredReason,
                            NewValue = request.ActionReason
                        });
                    }
                }
                else if (request.ServiceStatus == ServiceStatus.Dismissed)
                {
                    if (member.DismissedDate != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.DismissedDate),
                            OldValue = member.DismissedDate?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                    if (member.DismissedReason != request.ActionReason)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.DismissedReason),
                            OldValue = member.DismissedReason,
                            NewValue = request.ActionReason
                        });
                    }
                }
                else if (request.ServiceStatus == ServiceStatus.CDM)
                {
                    if (member.CdmDate != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.CdmDate),
                            OldValue = member.CdmDate?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                }
                else if (request.ServiceStatus == ServiceStatus.Absence)
                {
                    if (member.AbsenceStartedDate != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.AbsenceStartedDate),
                            OldValue = member.AbsenceStartedDate?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                }
                else if (request.ServiceStatus == ServiceStatus.Casualty)
                {
                    if (member.DateOfDeath != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.DateOfDeath),
                            OldValue = member.DateOfDeath?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }
                    if (member.BeneficiaryCity != request.BeneficiaryCity)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.BeneficiaryCity),
                            OldValue = member.BeneficiaryCity,
                            NewValue = request.BeneficiaryCity
                        });
                    }
                    if (member.BeneficiaryPhoneNumber != request.BeneficiaryPhoneNumber)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.BeneficiaryPhoneNumber),
                            OldValue = member.BeneficiaryPhoneNumber,
                            NewValue = request.BeneficiaryPhoneNumber
                        });
                    }
                }
                else if (request.ServiceStatus == ServiceStatus.Death)
                {
                    if (member.DateOfDeath != request.ActionDate)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.DateOfDeath),
                            OldValue = member.DateOfDeath?.ToString("U"),
                            NewValue = request.ActionDate?.ToString("U")
                        });
                    }

                    if (member.ReasonOfDeath != request.ActionReason)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.ReasonOfDeath),
                            OldValue = member.ReasonOfDeath,
                            NewValue = request.ActionReason
                        });
                    }

                    if (member.BeneficiaryCity != request.BeneficiaryCity)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.BeneficiaryCity),
                            OldValue = member.BeneficiaryCity,
                            NewValue = request.BeneficiaryCity
                        });
                    }

                    if (member.BeneficiaryPhoneNumber != request.BeneficiaryPhoneNumber)
                    {
                        memberProtection.ProtectionDetails.Add(new MemberProtectionDetail
                        {
                            KeyName = nameof(member.BeneficiaryPhoneNumber),
                            OldValue = member.BeneficiaryPhoneNumber,
                            NewValue = request.BeneficiaryPhoneNumber
                        });
                    }
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
