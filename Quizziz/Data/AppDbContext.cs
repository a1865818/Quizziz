// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

using Quizziz.Models;

namespace Quizziz.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizResponse> QuizResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Question)
                .WithMany()
                .HasForeignKey(qq => qq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Preserve questions even if removed from quiz

            modelBuilder.Entity<QuizResponse>()
                .HasOne(qr => qr.QuizAttempt)
                .WithMany(qa => qa.Responses)
                .HasForeignKey(qr => qr.QuizAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            Seed.SeedData(modelBuilder);
        }
    }
}