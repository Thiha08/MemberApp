using MemberApp.Model.Constants;
using System;

namespace MemberApp.Model.Entities
{
    public class Member : EntityBase, IAggregateRoot
    {
        public string ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public string PermanentContactNumber { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public string Rank { get; set; }
        public string BCNumber { get; set; }
        public string Battalion { get; set; }
        public string Division { get; set; }
        public DateTime? ActionDate { get; set; }
        public string ActionReason { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string BeneficiaryPhoneNumber { get; set; }
        public PermissionStatus PermissionStatus { get; set; }
        public DateTime? PermissionDate { get; set; }
        public string EditOTP { get; set; }
        public ApplicationUser User { get; set; }
    }
}
