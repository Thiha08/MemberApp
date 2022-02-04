using MemberApp.Model.Entities;
using System;
using System.Linq;

namespace MemberApp.Data.Infrastructure
{
    public static class DbInitializer
    {
        private static MemberAppContext context;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = (MemberAppContext)serviceProvider.GetService(typeof(MemberAppContext));

            InitializeMemberRoles();
        }

        private static void InitializeMemberRoles()
        {
            if (!context.Roles.Any())
            {
                // create roles
                context.Roles.AddRange(new Role[]
                {
                    new Role()
                    {
                        Name="Admin"
                    }
                });

                context.SaveChanges();
            }

            if (!context.Members.Any())
            {
                context.Members.Add(new Member()
                {
                    PhoneNumber = "9424432870",
                    Username = "9424432870",
                    FullName = "Thiha",
                    HashedPassword = "9wsmLgYM5Gu4zA/BSpxK2GIBEWzqMPKs8wl2WDBzH/4=",
                    Salt = "GTtKxJA6xJuj3ifJtTXn9Q==",
                    IsLocked = false,
                    DateCreated = DateTime.Now
                });

                // create user-admin
                context.MemberRoles.AddRange(new MemberRole[] 
                {
                    new MemberRole() 
                    {
                        RoleId = 1, // admin
                        MemberId = 1  // thiha
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
