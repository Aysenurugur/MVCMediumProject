using MediumProject.EF_MediumProject_CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediumProject.EF_MediumProject_CodeFirst.Context
{
    public class MediumContext : DbContext
    {
        public MediumContext(DbContextOptions<MediumContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<StoryTopic> StoryTopics { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID).HasName("PK_UserID");

                entity.Property(e => e.FullName).HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2");
                entity.Property(e => e.EmailAddress).HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.UserName).HasMaxLength(20)
                    .IsRequired();

                entity.HasIndex(e => e.EmailAddress).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Url).IsUnique();
            });
            modelBuilder.Entity<Story>(entity =>
            {
                entity.HasKey(e => e.StoryID).HasName("PK_StoryID");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Stories)
                    .HasForeignKey(e => e.UserID)
                    .HasConstraintName("FK_Story_User");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
                entity.Property(e => e.UserID).IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2");
                entity.Property(e => e.Title).HasMaxLength(200)
                    .IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasKey(e => e.TopicID).HasName("PK_TopicID");

                entity.Property(e => e.Title).HasMaxLength(100)
                    .IsRequired();
                entity.HasIndex(e => e.Title).IsUnique();

            });
            modelBuilder.Entity<StoryTopic>(entity =>
            {
                entity.HasOne(e => e.Story)
                    .WithMany(s => s.StoryTopics)
                    .HasForeignKey(e => e.StoryID)
                    .HasConstraintName("FK_Story_Topic");

                entity.HasOne(e => e.Topic)
                    .WithMany(t => t.StoryTopics)
                    .HasForeignKey(e => e.TopicID)
                    .HasConstraintName("FK_Topic_Story");

                entity.HasKey(e => new { e.StoryID, e.TopicID }).HasName("PK_Story_Topic");
                entity.HasIndex(e => e.StoryID, "StoryID");
                entity.HasIndex(e => e.TopicID, "TopicID");
            });
            modelBuilder.Entity<UserTopic>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(s => s.UserTopics)
                    .HasForeignKey(e => e.UserID)
                    .HasConstraintName("FK_User_Topic");

                entity.HasOne(e => e.Topic)
                    .WithMany(t => t.UserTopics)
                    .HasForeignKey(e => e.TopicID)
                    .HasConstraintName("FK_Topic_User");

                entity.HasKey(e => new { e.UserID, e.TopicID }).HasName("PK_User_Topic");
                entity.HasIndex(e => e.UserID, "UserID");

                entity.HasIndex(e => e.TopicID, "TopicID");
            });
            base.OnModelCreating(modelBuilder);
        }
    }

}
