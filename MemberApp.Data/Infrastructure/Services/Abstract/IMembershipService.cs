using MemberApp.Data.Infrastructure.Core;
using MemberApp.Model.Entities;
using System.Collections.Generic;

namespace MemberApp.Data.Infrastructure.Services.Abstract
{
    public interface IMembershipService
    {
        MembershipContext ValidateMember(string username, string password);
        Member CreateMember(string username, string phoneNumber, string password, int[] roles);
        Member GetMember(int memberId);
        List<Role> GetMemberRoles(string username);
    }
}
