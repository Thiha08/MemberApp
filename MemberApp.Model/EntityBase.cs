using System;

namespace MemberApp.Model
{
    public abstract class EntityBase
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string UpdatedBy { get; set; }

        public bool Status { get; set; }

    }
}
