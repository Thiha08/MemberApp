using MemberApp.Model.Constants;
using System.Collections.Generic;

namespace MemberApp.Web.ViewModels.Members
{
    public class MemberProtectionViewModel
    {
        public long Id { get; set; }

        public long MemberId { get; set; }

        public ProtectionStatus Status { get; set; }

        public List<MemberProtectionDetailViewModel> MemberProtectionDetails { get; set; } = new List<MemberProtectionDetailViewModel>();
    }
}
