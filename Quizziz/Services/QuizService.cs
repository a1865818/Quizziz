using System.Collections.Generic;
using System.Threading.Tasks;
using Quizziz.Models;
using Quizziz.Interfaces;
using Quizziz.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Quizziz.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;
        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            return await _context.Quizzes.
                Include(q => q.QuizQuestions).
                ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _context.Quizzes.FindAsync(id);
        }
        public async Task<Quiz> GetQuizWithQuestionsAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Question)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            quiz.CreatedAt = System.DateTime.Now;
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }
        public async Task UpdateQuizAsync(Quiz quiz)
        {
            quiz.UpdatedAt = System.DateTime.Now;
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteQuizAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddQuestionToQuizAsync(int quizId, int questionId, int order)
        {
            var quizQuestion = await _context.QuizQuestions.FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

            if (quizQuestion == null)
            {
                quizQuestion = new QuizQuestion
                {
                    QuizId = quizId,
                    QuestionId = questionId,
                    Order = order
                };
                _context.QuizQuestions.Add(quizQuestion);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveQuestionFromQuizAsync(int quizId, int questionId)
        {
            var quizQuestion = await _context.QuizQuestions.FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
            if (quizQuestion != null)
            {
                _context.QuizQuestions.Remove(quizQuestion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
