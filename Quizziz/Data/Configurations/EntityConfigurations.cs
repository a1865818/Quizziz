using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizziz.Models;

namespace Quizziz.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Text).IsRequired();
            builder.Property(q => q.CreatedAt).ValueGeneratedOnAdd();

            builder.HasMany(q => q.Answers)
                   .WithOne(a => a.Question)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Text).IsRequired();
            builder.Property(a => a.IsCorrect).IsRequired();

            builder.HasOne(a => a.Question)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quizzes");
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Title).IsRequired();
            builder.Property(q => q.Description).IsRequired();
            builder.Property(q => q.TimeLimit).IsRequired(false);
            builder.Property(q => q.CreatedAt).ValueGeneratedOnAdd();

            builder.HasMany(q => q.QuizQuestions)
                   .WithOne(qq => qq.Quiz)
                   .HasForeignKey(qq => qq.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.ToTable("QuizQuestions");
            builder.HasKey(qq => qq.Id);
            builder.Property(qq => qq.Order).IsRequired();

            builder.HasOne(qq => qq.Quiz)
                   .WithMany(q => q.QuizQuestions)
                   .HasForeignKey(qq => qq.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qq => qq.Question)
                   .WithMany()
                   .HasForeignKey(qq => qq.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict); // Preserve questions even if removed from quiz
        }
    }

    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.ToTable("QuizAttempts");
            builder.HasKey(qa => qa.Id);
            builder.Property(qa => qa.StartedAt).IsRequired();
            builder.Property(qa => qa.CompletedAt).IsRequired(false);
            builder.Property(qa => qa.Score).IsRequired(false);
            builder.Property(qa => qa.QuestionOrder).IsRequired(false);

            builder.HasOne(qa => qa.Quiz)
                   .WithMany()
                   .HasForeignKey(qa => qa.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(qa => qa.Responses)
                   .WithOne(qr => qr.QuizAttempt)
                   .HasForeignKey(qr => qr.QuizAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class QuizResponseConfiguration : IEntityTypeConfiguration<QuizResponse>
    {
        public void Configure(EntityTypeBuilder<QuizResponse> builder)
        {
            builder.ToTable("QuizResponses");
            builder.HasKey(qr => qr.Id);
            builder.Property(qr => qr.IsCorrect).IsRequired();

            builder.HasOne(qr => qr.QuizAttempt)
                   .WithMany(qa => qa.Responses)
                   .HasForeignKey(qr => qr.QuizAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qr => qr.Question)
                   .WithMany()
                   .HasForeignKey(qr => qr.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(qr => qr.SelectedAnswer)
                   .WithMany()
                   .HasForeignKey(qr => qr.SelectedAnswerId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}