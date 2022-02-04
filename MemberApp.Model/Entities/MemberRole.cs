namespace MemberApp.Model.Entities
{
    public class MemberRole : IEntityBase
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
