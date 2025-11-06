using ForumWebsite.Models.Activity;
using ForumWebsite.Models.Reply;
using ForumWebsite.Models.Thread;
using ForumWebsite.Models.User;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ForumWebsite.Data
{
    public class MyDBContextApplication : DbContext
    {
        public MyDBContextApplication() { }

        public MyDBContextApplication(DbContextOptions<MyDBContextApplication> options) : base(options) { }
        #region
        public DbSet<ThreadModel> Threads { get; set; }
        public DbSet<ReplyModel> Replies { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<BookmarkModel> Bookmarks { get; set; }
        public DbSet<ReportModel> ReportModels { get; set; }
        public DbSet<UserActivityModel> UserActivities { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
        public DbSet<UserModel> Users { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== UserActivity =====
            modelBuilder.Entity<UserActivityModel>()
                .HasOne(k => k.User)
                .WithMany()
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserActivityModel>()
                .HasOne(k => k.Thread)
                .WithMany()
                .HasForeignKey(k => k.ThreadID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserActivityModel>()
                .HasOne(k => k.Reply)
                .WithMany()
                .HasForeignKey(k => k.ReplyID)
                .OnDelete(DeleteBehavior.NoAction);

            // ===== Thread =====
            modelBuilder.Entity<ThreadModel>()
                .HasOne(k => k.User)
                .WithMany(u => u.Threads)
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.Cascade); // delete threads when user deleted

            modelBuilder.Entity<ThreadModel>()
                .HasOne(k => k.Category)
                .WithMany(u => u.Threads)
                .HasForeignKey(k => k.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // category cannot be deleted if has threads

            modelBuilder.Entity<ThreadModel>()
                .HasOne(t => t.Image)
                .WithOne(i => i.Thread)
                .HasForeignKey<ImageModel>(i => i.ThreadID)
                .OnDelete(DeleteBehavior.Cascade); // delete thread image

            // ===== Reply =====
            modelBuilder.Entity<ReplyModel>()
                .HasOne(k => k.Thread)
                .WithMany(u => u.Replies)
                .HasForeignKey(k => k.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReplyModel>()
                .HasOne(k => k.ParentReply)
                .WithMany(u => u.ChildReplies)
                .HasForeignKey(k => k.ParentReplyID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReplyModel>()
                .HasOne(k => k.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReplyModel>()
                .HasOne(r => r.Image)
                .WithOne(i => i.Reply)
                .HasForeignKey<ImageModel>(i => i.ReplyID)
                .OnDelete(DeleteBehavior.NoAction); // delete reply image

            // ===== Bookmark =====
            modelBuilder.Entity<BookmarkModel>()
                .HasOne(k => k.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookmarkModel>()
                .HasOne(k => k.Thread)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(k => k.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Like =====
            modelBuilder.Entity<LikeModel>()
                .HasOne(k => k.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.NoAction); // prevents cascade loop

            modelBuilder.Entity<LikeModel>()
                .HasOne(k => k.Thread)
                .WithMany(u => u.Likes)
                .HasForeignKey(k => k.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Report =====
            modelBuilder.Entity<ReportModel>()
                .HasOne(k => k.Thread)
                .WithMany(u => u.Reports)
                .HasForeignKey(k => k.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReportModel>()
                .HasOne(k => k.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            // ===== Status =====
            modelBuilder.Entity<ThreadModel>()
                .HasOne(k => k.Status)
                .WithMany(u => u.Threads)
                .HasForeignKey(k => k.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserModel>()
                .HasOne(k => k.Status)
                .WithMany(u => u.Users)
                .HasForeignKey(k => k.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportModel>()
                .HasOne(k => k.Status)
                .WithMany(u => u.Reports)
                .HasForeignKey(k => k.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReplyModel>()
                .HasOne(k => k.Status)
                .WithMany(u => u.Replies)
                .HasForeignKey(k => k.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Image =====
            modelBuilder.Entity<ImageModel>()
                .HasOne(i => i.Thread)
                .WithOne(t => t.Image)
                .HasForeignKey<ImageModel>(i => i.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ImageModel>()
                .HasOne(i => i.Reply)
                .WithOne(r => r.Image)
                .HasForeignKey<ImageModel>(i => i.ReplyID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
