using MemberApp.Model.Constants;

namespace MemberApp.Web.ViewModels.Members
{
    public class MemberProtectionDetailViewModel
    {
        public long Id { get; set; }

        public string KeyName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public ProtectionStatus Status { get; set; }
    }
}
