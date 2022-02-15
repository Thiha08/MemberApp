using MemberApp.Model.Constants;
using System;

namespace MemberApp.Web.ViewModels.Members
{
    public class MemberViewModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentJob { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public string Rank { get; set; }
        public string BCNumber { get; set; }
        public string LastBattalion { get; set; }
        public string ResignationDate { get; set; }
        public string ResignationReason { get; set; }
        public string RetiredDate { get; set; }
        public string RetiredReason { get; set; }
        public string DismissedDate { get; set; }
        public string DismissedReason { get; set; }
        public string CdmDate { get; set; }
        public string AbsenceStartedDate { get; set; }
        public string DateOfDeath { get; set; }
        public string ReasonOfDeath { get; set; }
        public string BeneficiaryCity { get; set; }
        public string BeneficiaryPhoneNumber { get; set; }
    }
}
