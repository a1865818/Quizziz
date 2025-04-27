using Microsoft.EntityFrameworkCore;
using Quizziz.Models;

namespace Quizziz.Data
{
    public static class Seed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Questions
            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, Text = "What is the capital of France?" },
                new Question { Id = 2, Text = "What is 2 + 2?" },
                new Question { Id = 3, Text = "Who painted the Mona Lisa?" }
            );

            // Seed Answers
            modelBuilder.Entity<Answer>().HasData(
                // Answers for Question 1
                new Answer { Id = 1, QuestionId = 1, Text = "Paris", IsCorrect = true },
                new Answer { Id = 2, QuestionId = 1, Text = "London", IsCorrect = false },
                new Answer { Id = 3, QuestionId = 1, Text = "Berlin", IsCorrect = false },
                new Answer { Id = 4, QuestionId = 1, Text = "Madrid", IsCorrect = false },

                // Answers for Question 2
                new Answer { Id = 5, QuestionId = 2, Text = "3", IsCorrect = false },
                new Answer { Id = 6, QuestionId = 2, Text = "4", IsCorrect = true },
                new Answer { Id = 7, QuestionId = 2, Text = "5", IsCorrect = false },
                new Answer { Id = 8, QuestionId = 2, Text = "22", IsCorrect = false },

                // Answers for Question 3
                new Answer { Id = 9, QuestionId = 3, Text = "Leonardo da Vinci", IsCorrect = true },
                new Answer { Id = 10, QuestionId = 3, Text = "Pablo Picasso", IsCorrect = false },
                new Answer { Id = 11, QuestionId = 3, Text = "Vincent van Gogh", IsCorrect = false },
                new Answer { Id = 12, QuestionId = 3, Text = "Michelangelo", IsCorrect = false }
            );

            // Seed Quizzes
            modelBuilder.Entity<Quiz>().HasData(
                new Quiz { Id = 1, Title = "General Knowledge Quiz", Description = "Test your general knowledge!" }
            );

            // Seed QuizQuestions
            modelBuilder.Entity<QuizQuestion>().HasData(
                new QuizQuestion { Id = 1, QuizId = 1, QuestionId = 1, Order = 1 },
                new QuizQuestion { Id = 2, QuizId = 1, QuestionId = 2, Order = 2 },
                new QuizQuestion { Id = 3, QuizId = 1, QuestionId = 3, Order = 3 }
            );
        }
    }
}
