using System;
using System.Collections.Generic;

namespace MemberApp.Model.Entities
{
    public class Member : IEntityBase
    {
        public Member()
        {
            MemberRoles = new List<MemberRole>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<MemberRole> MemberRoles { get; set; }
    }
}
