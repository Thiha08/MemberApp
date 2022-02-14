using MemberApp.Model.Constants;
using System;
using System.Collections.Generic;

namespace MemberApp.Model.Entities
{
    public class Member : EntityBase, IAggregateRoot
    {
        public string ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentJob { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public string Rank { get; set; }
        public string BCNumber { get; set; }
        public string LastBattalion { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string ResignationReason { get; set; }
        public DateTime? RetiredDate { get; set; }
        public string RetiredReason { get; set; }
        public DateTime? DismissedDate { get; set; }
        public string DismissedReason { get; set; }
        public DateTime? CdmDate { get; set; }
        public DateTime? AbsenceStartedDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public DateTime? ReasonOfDeath { get; set; }
        public string BeneficiaryCity { get; set; }
        public string BeneficiaryPhoneNumber { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<MemberProtection> Protections { get; set; }
    }
}
