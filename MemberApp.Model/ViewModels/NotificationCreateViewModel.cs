using MemberApp.Model.Constants;

namespace MemberApp.Model.ViewModels
{
    public class NotificationCreateViewModel
    {
        public string[] Recipients { get; set; }

        public string Content { get; set; }

        public string TargetUrl { get; set; }

        public NotificationCategory Category { get; set; }
    }
}
