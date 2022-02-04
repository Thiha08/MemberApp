using System.ComponentModel.DataAnnotations;

namespace MemberApp.Web.ViewModels.Params
{
    public class PagingParams
    {
        [Required]
        public int PageNo { get; set; }

        public int PageSize { get; set; }
    }
}
