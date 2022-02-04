using MemberApp.Data.Abstract;
using MemberApp.Model.Entities;

namespace MemberApp.Data.Repositories
{
    public class MemberRepository : EntityBaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(MemberAppContext context)
            : base(context) { }
    }
}
