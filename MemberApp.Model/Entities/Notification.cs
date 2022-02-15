using MemberApp.Model.Constants;
using System.Collections.Generic;

namespace MemberApp.Model.Entities
{
    public class Notification : EntityBase, IAggregateRoot
    {
        public string Content { get; set; }

        public string TargetUrl { get; set; }

        public NotificationCategory Category { get; set; }

        public ICollection<NotificationRecipient> Recipients { get; set; }
    }
}
