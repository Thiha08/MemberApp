using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.Core;
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
        private IMemberRepository _memberRepository;

        int page = 1;
        int pageSize = 4;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public IActionResult Get()
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            int currentPage = page;
            int currentPageSize = pageSize;
            var totalMembers = _memberRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalMembers / pageSize);

            IEnumerable<Member> _schedules = _memberRepository
                .GetAll()
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            Response.AddPagination(page, pageSize, totalMembers, totalPages);

            IEnumerable<MemberViewModel> _membersVM = new List<MemberViewModel>();

            return new OkObjectResult(_membersVM);
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
