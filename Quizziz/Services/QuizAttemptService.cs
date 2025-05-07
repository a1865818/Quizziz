using Quizziz.Models;
using Quizziz.Interfaces;
using Quizziz.Data;
using Microsoft.EntityFrameworkCore;

namespace Quizziz.Services
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly AppDbContext _context;
        public QuizAttemptService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuizAttempt> StartQuizAttemptAsync(int quizId)
        {
            // Get the quiz with questions
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                return null;

            // Create a new attempt
            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                StartedAt = DateTime.Now,
                CompletedAt = null,
                Score = null
            };

           
            var questionIds = quiz.QuizQuestions.Select(qq => qq.QuestionId).ToList();

            if (quiz.RandomizeQuestions)
            {
                // Randomize the question order
                Random rng = new Random();
                questionIds = questionIds.OrderBy(x => rng.Next()).ToList();
            }
            else
            {
                // Use the defined order
                questionIds = quiz.QuizQuestions.OrderBy(qq => qq.Order).Select(qq => qq.QuestionId).ToList();
            }

            // Store the order as a comma-separated string
            attempt.QuestionOrder = string.Join(",", questionIds);

            // Add to database and save
            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            // Load the full quiz data for the new attempt
            return await GetQuizAttemptAsync(attempt.Id);
        }


        public async Task<QuizAttempt> GetQuizAttemptAsync(int attemptId)
        {
            return await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                    .ThenInclude(qq => qq.Question)
                    .ThenInclude(q => q.Answers)
                .Include(qa => qa.Responses)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);
        }

        public async Task<QuizResponse> SaveQuizResponseAsync(int attemptId, int questionId, int answerId)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null || answer.QuestionId != questionId)
            {
                return null;
            }

            var existingResponse = await _context.QuizResponses
                .FirstOrDefaultAsync(r => r.QuizAttemptId == attemptId && r.QuestionId == questionId);

            if (existingResponse != null)
            {
                existingResponse.SelectedAnswerId = answerId;
                existingResponse.IsCorrect = answer.IsCorrect;
                _context.QuizResponses.Update(existingResponse);
                await _context.SaveChangesAsync();
                return existingResponse;
            }
            else
            {
                var response = new QuizResponse
                {
                    QuizAttemptId = attemptId,
                    QuestionId = questionId,
                    SelectedAnswerId = answerId,
                    IsCorrect = answer.IsCorrect
                };
                _context.QuizResponses.Add(response);
                await _context.SaveChangesAsync();
                return response;
            }
        }

        public async Task<QuizAttempt> CompleteQuizAttemptAsync(int attemptId)
        {
            var attempt = await _context.QuizAttempts
                .Include(qa => qa.Responses)
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);

            if (attempt == null)
            {
                return null;
            }

            // Mark the quiz as completed
            attempt.CompletedAt = DateTime.Now;

            // Calculate the score
            int totalQuestions = attempt.Quiz.QuizQuestions.Count;
            int correctAnswers = attempt.Responses.Count(r => r.IsCorrect);
            attempt.Score = totalQuestions > 0 ? (correctAnswers * 100 / totalQuestions) : 0;

            _context.QuizAttempts.Update(attempt);
            await _context.SaveChangesAsync(); // Save changes to the database

            return attempt;
        }




        public async Task<IEnumerable<QuizAttempt>> GetAllQuizAttemptsAsync()
        {
            return await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .ToListAsync();
        }

    }


}
