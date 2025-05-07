using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quizziz.Data;
using Quizziz.Models;

namespace Quizziz.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            question.CreatedAt = System.DateTime.Now;
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task UpdateQuestionAsync(Question updatedQuestion)
        {
            var existingQuestion = await _context.Questions.FindAsync(updatedQuestion.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Text = updatedQuestion.Text;
                existingQuestion.UpdatedAt = System.DateTime.Now;
               
                _context.Questions.Update(existingQuestion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Quiz>> GetQuizzesUsingQuestionAsync(int questionId)
        {
            return await _context.QuizQuestions
                .Where(qq => qq.QuestionId == questionId)
                .Select(qq => qq.Quiz)
                .Distinct()
                .ToListAsync();
        }
    }
}