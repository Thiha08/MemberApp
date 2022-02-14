using MemberApp.Model.Constants;
using System;

namespace MemberApp.Web.ViewModels.RequestDTOs
{
    public class MemeberUpdateRequest
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Rank { get; set; }
        public string CurrentCity { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public string BCNumber { get; set; }
        public string LastBattalion { get; set; }
        public string CurrentJob { get; set; }
        public string BeneficiaryCity { get; set; }
        public string BeneficiaryPhoneNumber { get; set; }
        public DateTime? ActionDate { get; set; }
        public string ActionReason { get; set; }
    }
}
