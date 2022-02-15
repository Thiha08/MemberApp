using System;

namespace MemberApp.Model.Entities
{
    public class NotificationRecipient : EntityBase, IAggregateRoot
    {
        public string RecipientUserName { get; set; }

        public bool IsReceived { get; set; }

        public DateTime? ReceivedAt { get; set; }

        public bool IsSent { get; set; }

        public bool IsPopupHappened { get; set; }

        public DateTime? SentAt { get; set; }

        public long NotificationId { get; set; }

        public Notification Notification { get; set; }
    }
}
