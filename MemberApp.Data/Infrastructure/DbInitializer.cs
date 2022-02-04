using MemberApp.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MemberApp.Data.Infrastructure
{
    public static class DbInitializer
    {
        private static MemberAppContext context;

        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                context = serviceScope.ServiceProvider.GetService<MemberAppContext>();

                InitializeMemberRoles();

                // Seed the database.
            }
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

                context.SaveChanges();

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
