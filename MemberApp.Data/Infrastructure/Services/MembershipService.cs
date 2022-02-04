using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace MemberApp.Data.Infrastructure.Services
{
    public class MembershipService : IMembershipService
    {
        #region Variables
        private readonly IMemberRepository _memberRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMemberRoleRepository _memberRoleRepository;
        private readonly IEncryptionService _encryptionService;
        #endregion

        public MembershipService(
            IMemberRepository memberRepository,
            IRoleRepository roleRepository,
            IMemberRoleRepository memberRoleRepository,
            IEncryptionService encryptionService)
        {
            _memberRepository = memberRepository;
            _roleRepository = roleRepository;
            _memberRoleRepository = memberRoleRepository;
            _encryptionService = encryptionService;
        }

        #region IMembershipService Implementation

        public Member CreateMember(string username, string phoneNumber, string password, int[] roles)
        {
            var existingMember = _memberRepository.GetSingleByUsername(username);

            if (existingMember != null)
            {
                throw new Exception("Username is already in use");
            }

            var passwordSalt = _encryptionService.CreateSalt();

            var member = new Member()
            {
                Username = username,
                Salt = passwordSalt,
                PhoneNumber = phoneNumber,
                IsLocked = false,
                HashedPassword = _encryptionService.EncryptPassword(password, passwordSalt),
                DateCreated = DateTime.Now
            };

            _memberRepository.Add(member);

            _memberRepository.Commit();

            if (roles != null || roles.Length > 0)
            {
                foreach (var role in roles)
                {
                    addMemberToRole(member, role);
                }
            }

            _memberRepository.Commit();

            return member;
        }

        public Member GetMember(int memberId)
        {
            return _memberRepository.GetSingle(memberId);
        }

        public List<Role> GetMemberRoles(string username)
        {
            List<Role> _result = new List<Role>();

            var existingMember = _memberRepository.GetSingleByUsername(username);

            if (existingMember != null)
            {
                foreach (var memberRole in existingMember.MemberRoles)
                {
                    _result.Add(memberRole.Role);
                }
            }

            return _result.Distinct().ToList();
        }

        public MembershipContext ValidateMember(string username, string password)
        {
            var membershipCtx = new MembershipContext();

            var member = _memberRepository.GetSingleByUsername(username);
            if (member != null && isMemberValid(member, password))
            {
                var userRoles = GetMemberRoles(member.Username);
                membershipCtx.Member = member;

                var identity = new GenericIdentity(member.Username);
                membershipCtx.Principal = new GenericPrincipal(
                    identity,
                    userRoles.Select(x => x.Name).ToArray());
            }

            return membershipCtx;
        }

        #endregion

        #region Helper methods
        private void addMemberToRole(Member user, int roleId)
        {
            var role = _roleRepository.GetSingle(roleId);
            if (role == null)
                throw new Exception("Role doesn't exist.");

            var memberRole = new MemberRole()
            {
                RoleId = role.Id,
                MemberId = user.Id
            };
            _memberRoleRepository.Add(memberRole);

            _memberRoleRepository.Commit();
        }

        private bool isPasswordValid(Member member, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, member.Salt), member.HashedPassword);
        }

        private bool isMemberValid(Member member, string password)
        {
            if (isPasswordValid(member, password))
            {
                return !member.IsLocked;
            }

            return false;
        }
        #endregion



    }
}
