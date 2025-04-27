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
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Question)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                return null;
            }

            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                StartedAt = DateTime.Now
            };

            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
            return attempt;
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
                .Include (qa => qa.Responses)
                .Include (qa => qa.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);

            if (attempt == null)
            {
                return null;
            }
            attempt.CompletedAt = DateTime.Now;

            int totalQuestions = attempt.Quiz.QuizQuestions.Count;
            int correctAnswers = attempt.Responses.Count(r => r.IsCorrect);
            attempt.Score = totalQuestions > 0 ? (correctAnswers * 100 / totalQuestions) : 0;
            
            _context.QuizAttempts.Update(attempt);
            await _context.SaveChangesAsync();

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
