using System.Collections.Generic;

namespace MemberApp.Model.Entities
{
    public class Member : EntityBase, IAggregateRoot
    {
        public string ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<MemberProtection> Protections { get; set; }
    }
}
