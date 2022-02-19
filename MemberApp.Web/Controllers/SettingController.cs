using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberApp.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SettingController : BaseController
    {
        public SettingController(
            IRepository<AuditLog> loggingRepository)
            : base(loggingRepository)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
