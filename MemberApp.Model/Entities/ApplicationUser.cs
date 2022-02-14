using Microsoft.AspNetCore.Identity;
using System;

namespace MemberApp.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsLocked { get; set; }
        public string OTPCode { get; set; }
        public DateTime? OTPCodeExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string UpdatedBy { get; set; }
        public bool IsConfirmedByAdmin { get; set; }
        public bool Status { get; set; }
    }
}
