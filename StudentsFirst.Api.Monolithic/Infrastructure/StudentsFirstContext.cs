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
            modelBuilder.Entity<UserGroupMembership>().HasKey(ugm => new { ugm.UserId, ugm.GroupId });
            modelBuilder.Entity<UserGroupMembership>().HasOne<User>().WithMany().HasForeignKey(ugm => ugm.UserId);
            modelBuilder.Entity<UserGroupMembership>().HasOne<Group>().WithMany().HasForeignKey(ugm => ugm.GroupId);

            modelBuilder.Entity<Assignment>()
                .HasMany<AssignmentResource>(a => a.Resources).WithOne().HasForeignKey(ar => ar.AssignmentId);
            modelBuilder.Entity<Assignment>()
                .HasMany<AssignmentSubmission>(a => a.Submissions).WithOne().HasForeignKey(@as => @as.AssignmentId);

            modelBuilder.Entity<AssignmentSubmission>().HasOne<User>().WithMany().HasForeignKey(@as => @as.AssigneeId);
            modelBuilder.Entity<AssignmentSubmission>()
                .HasMany<AssignmentAttachment>(@as => @as.Attachments).WithOne().HasForeignKey(aa => aa.SubmissionId);
        }
    }
}
