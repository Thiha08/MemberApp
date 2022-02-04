using MemberApp.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MemberApp.Data
{
    public class MemberAppContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<Error> Errors { get; set; }

        public MemberAppContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Member
            modelBuilder.Entity<Member>().Property(m => m.Username).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Member>().Property(m => m.FullName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Member>().Property(m => m.PhoneNumber).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Member>().Property(m => m.HashedPassword).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Member>().Property(m => m.Salt).IsRequired().HasMaxLength(200);

            // MemberRole
            modelBuilder.Entity<MemberRole>().Property(mr => mr.MemberId).IsRequired();
            modelBuilder.Entity<MemberRole>().Property(mr => mr.RoleId).IsRequired();

            // Role
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(50);
        }

    }
}
