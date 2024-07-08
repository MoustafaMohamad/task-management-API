using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using task_management_API.Models;
using Task = task_management_API.Models.Task;

namespace task_management_API.DBContext
{
    public class TaskMangerContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        public TaskMangerContext(DbContextOptions<TaskMangerContext> options) : base(options)
        {
        }


    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().ToTable("Task")
                .HasKey(t => t.TaskId);

            modelBuilder.Entity<TeamMember>().ToTable("TeamMember")
                .HasKey(tm => tm.MemberId);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.TeamMember)
                .WithMany(m => m.Tasks)
                .HasForeignKey(t => t.MemberId).OnDelete(DeleteBehavior.SetNull);

         

            base.OnModelCreating(modelBuilder);
        }
    }
}
