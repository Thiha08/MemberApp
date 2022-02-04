using MemberApp.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MemberApp.Data
{
    public class MemberAppContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

        public MemberAppContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Member>()
                .Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();
        }

    }
}
