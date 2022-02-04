using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;

namespace MemberApp.Data.Repositories
{
    public class MemberRoleRepository : EntityBaseRepository<MemberRole>, IMemberRoleRepository
    {
        public MemberRoleRepository(MemberAppContext context) : base(context)
        {
        }
    }
}
