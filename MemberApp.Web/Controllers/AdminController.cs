using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
    }
}
