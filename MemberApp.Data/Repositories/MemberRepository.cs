using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;
using System.Collections.Generic;

namespace MemberApp.Data.Repositories
{
    public class MemberRepository : EntityBaseRepository<Member>, IMemberRepository
    {
        IRoleRepository _roleReposistory;

        public MemberRepository(MemberAppContext context, IRoleRepository roleReposistory)
            : base(context) 
        {
            _roleReposistory = roleReposistory;
        }
        public Member GetSingleByUsername(string username)
        {
            return this.GetSingle(x => x.Username == username);
        }

        public IEnumerable<Role> GetMemberRoles(string username)
        {
            List<Role> _roles = null;

            Member _member = this.GetSingle(u => u.Username == username, u => u.MemberRoles);
            if (_member != null)
            {
                _roles = new List<Role>();
                foreach (var _userRole in _member.MemberRoles)
                    _roles.Add(_roleReposistory.GetSingle(_userRole.RoleId));
            }

            return _roles;
        }
    }
}
