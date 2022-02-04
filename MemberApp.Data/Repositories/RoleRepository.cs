using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;

namespace MemberApp.Data.Repositories
{
    public class RoleRepository : EntityBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(MemberAppContext context)
            : base(context)
        { }
    }
}
