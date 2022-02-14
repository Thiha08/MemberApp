using MemberApp.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberApp.Model.Entities
{
    public class MemberProtectionDetail : EntityBase, IAggregateRoot, IDataProtection
    {
        public string KeyName { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public ProtectionStatus ProtectionStatus { get; set; }

        public long MemberProtectionId { get; set; }

        public MemberProtection MemberProtection { get; set; }
    }
}
