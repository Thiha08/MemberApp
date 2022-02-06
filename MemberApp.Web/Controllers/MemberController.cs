using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberApp.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class MemberController : BaseController
    {
        private readonly IRepository<Member> _memberRepository;


        public MemberController(
            IRepository<AuditLog> loggingRepository,
            IRepository<Member> memberRepository)
            : base(loggingRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<MemberOverviewViewModel> viewModel =
                await _memberRepository.AllIncluding(x => x.User)
                                       .Select(x => new MemberOverviewViewModel
                                       {
                                           Id = x.Id,
                                           CadetNumber = x.CadetNumber,
                                           FullName = x.FullName,
                                           PhoneNumber = x.User.PhoneNumber,
                                           Email = x.User.Email,
                                           CadetBattalion = x.CadetBattalion
                                       })
                                       .ToListAsync();

            return View(viewModel);
        }
    }
}
