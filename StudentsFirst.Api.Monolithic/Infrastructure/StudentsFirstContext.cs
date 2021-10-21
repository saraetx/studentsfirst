using Microsoft.EntityFrameworkCore;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure
{
    public class StudentsFirstContext : DbContext
    {
        public StudentsFirstContext(DbContextOptions options)
        : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<UserGroupMembership> UserGroupMemberships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            
            modelBuilder.Entity<Group>().HasKey(g => g.Id);
            
            modelBuilder.Entity<UserGroupMembership>().HasKey(ugm => new { ugm.UserId, ugm.GroupId });
            modelBuilder.Entity<UserGroupMembership>().HasOne<User>().WithMany().HasForeignKey(ugm => ugm.UserId);
            modelBuilder.Entity<UserGroupMembership>().HasOne<Group>().WithMany().HasForeignKey(ugm => ugm.GroupId);

            modelBuilder.Entity<Assignment>().HasKey(a => a.Id);

            modelBuilder.Entity<AssignmentResource>().HasKey(ar => ar.Id);
            modelBuilder.Entity<AssignmentResource>().HasOne<Assignment>().WithMany().HasForeignKey(ar => ar.AssignmentId);

            modelBuilder.Entity<AssignmentSubmission>().HasKey(@as => @as.Id);
            modelBuilder.Entity<AssignmentSubmission>().HasOne<User>().WithMany().HasForeignKey(@as => @as.AssigneeId);
            modelBuilder.Entity<AssignmentSubmission>().HasOne<Assignment>().WithMany().HasForeignKey(@as => @as.AssignmentId);

            modelBuilder.Entity<AssignmentAttachment>().HasKey(aa => aa.Id);
            modelBuilder.Entity<AssignmentAttachment>()
                .HasOne<AssignmentSubmission>().WithMany().HasForeignKey(aa => aa.SubmissionId);
        }
    }
}
