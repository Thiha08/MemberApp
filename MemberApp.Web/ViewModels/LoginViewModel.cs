using System.ComponentModel.DataAnnotations;

namespace MemberApp.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
