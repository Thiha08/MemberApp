using MemberApp.Model.Constants;

namespace MemberApp.Model.ViewModels
{
    public class MemberOverviewViewModel
    {
        public long Id { get; set; }
        public string BCNumber { get; set; }
        public string Rank { get; set; }
        public string FullName { get; set; }
        public ServiceStatus CurrentStatus { get; set; }
        public string Division { get; set; }
        public string PhoneNumber { get; set; }
        public string Request { get; set; }
    }
}
