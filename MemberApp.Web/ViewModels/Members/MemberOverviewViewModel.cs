namespace MemberApp.Web.ViewModels.Members
{
    public class MemberOverviewViewModel
    {
        public long Id { get; set; }
        public string BCNumber { get; set; }
        public string FullName { get; set; }
        public string LastBattalion { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentCity { get; set; }
        public bool IsLocked { get; set; }
        public bool IsConfirmedByAdmin { get; set; }
    }
}
