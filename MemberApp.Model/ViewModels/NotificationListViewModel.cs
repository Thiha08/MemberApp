using MemberApp.Model.Constants;

namespace MemberApp.Model.ViewModels
{
    public class NotificationListViewModel
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public string TargetUrl { get; set; }

        public NotificationCategory Category { get; set; }

        public string CreatedDate { get; set; }

        public bool IsReceived { get; set; }
    }
}
