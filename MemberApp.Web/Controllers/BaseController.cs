using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Web.ViewExtensions;
using MemberApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MemberApp.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public readonly IRepository<AuditLog> _loggingRepository;

        public BaseController(IRepository<AuditLog> loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isCurrentPatientLoggedIn = User != null && User.Claims != null && User.Claims.Any();

            await next();
        }

        internal void GenerateAlertMessage(bool isSuccessful, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var alertViewModel = new AlertViewModel
                {
                    Success = isSuccessful,
                    Message = message
                };

                TempData.Put("AlertViewModel", alertViewModel);
            }
        }
    }
}
