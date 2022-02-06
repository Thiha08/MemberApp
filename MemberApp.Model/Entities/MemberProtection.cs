using MemberApp.Model.Constants;

namespace MemberApp.Model.Entities
{
    public class MemberProtection : EntityBase, IAggregateRoot, IDataProtection
    {
        public long MemberId { get; set; }
        public string FullName { get; set; }
        public string CadetNumber { get; set; }
        public string CadetBattalion { get; set; }
        public ProtectionStatus ProtectionStatus { get; set; }
    }
}
