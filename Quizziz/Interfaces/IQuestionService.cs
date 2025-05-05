using System.Collections.Generic;
using System.Threading.Tasks;
using Quizziz.Models;

namespace Quizziz.Services
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
        Task<Question> CreateQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
        Task<List<Quiz>> GetQuizzesUsingQuestionAsync(int questionId);
    }
}