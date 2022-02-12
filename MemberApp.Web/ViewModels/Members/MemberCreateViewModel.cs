using System.ComponentModel.DataAnnotations;

namespace MemberApp.Web.ViewModels.Members
{
    public class MemberCreateViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
