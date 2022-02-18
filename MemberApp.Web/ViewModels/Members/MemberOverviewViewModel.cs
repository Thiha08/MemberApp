using System.ComponentModel.DataAnnotations;

namespace MemberApp.Web.ViewModels.Members
{
    public class MemberOverviewViewModel
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Display(Name = "BCNumber")]
        public string BCNumber { get; set; }

        [Display(Name = "Rank")]
        public string Rank { get; set; }

        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "CurrentStatus")]
        public string CurrentStatus { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Request")]
        public string Request { get; set; }
    }
}
