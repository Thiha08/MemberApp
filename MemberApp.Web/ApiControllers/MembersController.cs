using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemberApp.Web.ApiControllers
{
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILoggingRepository _loggingRepository;

        int page = 1;
        int pageSize = 4;

        public MembersController(IMemberRepository memberRepository, ILoggingRepository loggingRepository)
        {
            _memberRepository = memberRepository;
            _loggingRepository = loggingRepository;
        }

        [HttpGet("{page:int=0}/{pageSize=12}")]
        public PaginationSet<MemberViewModel> Get(int? page, int? pageSize)
        {
            PaginationSet<MemberViewModel> pagedSet = null;

            try
            {
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;

                List<Member> _members = null;
                int _totalMembers = new int();

                _members = _memberRepository
                    .GetAll()
                    .OrderBy(p => p.Id)
                    .Skip(currentPage * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();

                _totalMembers = _memberRepository.Count();

                IEnumerable<MemberViewModel> _membersVM = new List<MemberViewModel>();

                pagedSet = new PaginationSet<MemberViewModel>()
                {
                    Page = currentPage,
                    TotalCount = _totalMembers,
                    TotalPages = (int)Math.Ceiling((decimal)_totalMembers / currentPageSize),
                    Items = _membersVM
                };
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            return pagedSet;
        }

        [HttpGet("{id}", Name = "GetMember")]
        public IActionResult Get(int id)
        {
            Member _member = _memberRepository.GetSingle(s => s.Id == id);

            if (_member != null)
            {
                MemberViewModel _memberVM = new MemberViewModel();
                return new OkObjectResult(_memberVM);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/details", Name = "GetMemberDetails")]
        public IActionResult GetMemberDetails(int id)
        {
            Member _member = _memberRepository.GetSingle(s => s.Id == id);

            if (_member != null)
            {
                MemberDetailsViewModel _memberDetailsVM = new MemberDetailsViewModel();
                return new OkObjectResult(_memberDetailsVM);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
