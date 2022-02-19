using MemberApp.Model.Constants;

namespace MemberApp.Web.ViewModels.DTOs
{
    public class MemberOverviewDTO
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public ServiceStatus ServiceStatus { get; set; }
    }
}
