using MemberApp.Model.Entities;
using System.Security.Principal;

namespace MemberApp.Data.Infrastructure.Core
{
    public class MembershipContext
    {
        public IPrincipal Principal { get; set; }
        public Member Member { get; set; }
        public bool IsValid()
        {
            return Principal != null;
        }
    }
}
