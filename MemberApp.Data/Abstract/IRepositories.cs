using MemberApp.Model.Entities;
using System.Collections.Generic;

namespace MemberApp.Data.Abstract
{
    public interface ILoggingRepository : IEntityBaseRepository<Error> { }
    public interface IRoleRepository : IEntityBaseRepository<Role> { }
    public interface IMemberRepository : IEntityBaseRepository<Member> 
    {
        Member GetSingleByUsername(string username);
        IEnumerable<Role> GetMemberRoles(string username);
    }
    public interface IMemberRoleRepository : IEntityBaseRepository<MemberRole> { }
}
