namespace MemberApp.Web.ViewModels.DTOs
{
    public class MemberDetailsDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string ServiceStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Rank { get; set; }
        public string CurrentCity { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public string BCNumber { get; set; }
        public string LastBattalion { get; set; }
        public string CurrentJob { get; set; }
        public string ActionDate { get; set; }
        public string ActionReason { get; set; }
    }
}
