using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MemberApp.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class HomeController : BaseController
    {
        public HomeController(
            IRepository<AuditLog> loggingRepository)
            : base(loggingRepository)
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Member");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
