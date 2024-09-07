using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Models
{
   
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<Employee> Employees { get; set; }
            public DbSet<TaskItem> TaskItems { get; set; }
            public DbSet<Team> Teams { get; set; }
            public DbSet<TaskNotes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                //modelBuilder.Entity<Employee>()
                //.HasOne(e => e.Team)  // One employee can be part of one team
                //.WithMany(t => t.Employees)  // One team can have many employees
                //.HasForeignKey(e => e.TeamId)
                //.OnDelete(DeleteBehavior.SetNull);  // ON DELETE SET NULL behavior
                base.OnModelCreating(modelBuilder);
            }
        }
}
