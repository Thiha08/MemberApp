//using MemberApp.Data.Abstract;
//using MemberApp.Data.Infrastructure.Core;
//using MemberApp.Model.Entities;
//using MemberApp.Web.ViewModels;
//using MemberApp.Web.ViewModels.Params;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MemberApp.Web.ApiControllers
//{
//    [ApiController]
//    [Route("api/members")]
//    [Produces("application/json")]
//    [Consumes("application/json")]
//    public class MembersApiController : ControllerBase
//    {
//        private readonly IMemberRepository _memberRepository;
//        private readonly ILoggingRepository _loggingRepository;

//        public MembersApiController(IMemberRepository memberRepository, ILoggingRepository loggingRepository)
//        {
//            _memberRepository = memberRepository;
//            _loggingRepository = loggingRepository;
//        }

//        [HttpGet]
//        public PaginationSet<MemberViewModel> Get([FromQuery, BindRequired] string keywords, [FromQuery] PagingParams pagingParams)
//        {
//            PaginationSet<MemberViewModel> pagedSet = null;

//            try
//            {
//                List<Member> _members = null;
//                int _totalMembers = new int();

//                _members = _memberRepository
//                    .GetAll()
//                    .OrderBy(p => p.Id)
//                    .Skip((pagingParams.PageNo - 1) * pagingParams.PageSize)
//                    .Take(pagingParams.PageSize)
//                    .ToList();

//                _totalMembers = _memberRepository.Count();

//                IEnumerable<MemberViewModel> _membersVM = new List<MemberViewModel>();

//                pagedSet = new PaginationSet<MemberViewModel>()
//                {
//                    Page = pagingParams.PageNo,
//                    TotalCount = _totalMembers,
//                    TotalPages = (int)Math.Ceiling((decimal)_totalMembers / pagingParams.PageSize),
//                    Items = _membersVM
//                };
//            }
//            catch (Exception ex)
//            {
//                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
//                _loggingRepository.Commit();
//            }

//            return pagedSet;
//        }

//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            Member _member = _memberRepository.GetSingle(s => s.Id == id);

//            if (_member != null)
//            {
//                MemberViewModel _memberVM = new MemberViewModel();
//                return new OkObjectResult(_memberVM);
//            }
//            else
//            {
//                return NotFound();
//            }
//        }

//        [HttpGet("{id}/details")]
//        public IActionResult GetMemberDetails(int id)
//        {
//            Member _member = _memberRepository.GetSingle(s => s.Id == id);

//            if (_member != null)
//            {
//                MemberDetailsViewModel _memberDetailsVM = new MemberDetailsViewModel();
//                return new OkObjectResult(_memberDetailsVM);
//            }
//            else
//            {
//                return NotFound();
//            }
//        }
//    }
//}
