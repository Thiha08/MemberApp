using MemberApp.Model.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace MemberApp.Model.ViewModels
{
    public class MemberEditViewModel
    {
        public long Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

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
    }
}
