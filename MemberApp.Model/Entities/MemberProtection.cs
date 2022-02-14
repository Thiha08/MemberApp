using MemberApp.Model.Constants;
using System.Collections.Generic;

namespace MemberApp.Model.Entities
{
    public class MemberProtection : EntityBase, IAggregateRoot, IDataProtection
    {
        public long MemberId { get; set; }
        public ProtectionStatus ProtectionStatus { get; set; }
        public ICollection<MemberProtectionDetail> ProtectionDetails { get; set; }
    }
}
