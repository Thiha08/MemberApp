using Microsoft.Extensions.Logging;

namespace MemberApp.Model.Entities
{
    public class AuditLog : EntityBase, IAggregateRoot
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
