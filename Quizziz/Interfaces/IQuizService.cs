using System.Collections.Generic;
using Quizziz.Models;
using System.Threading.Tasks;

namespace Quizziz.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<Quiz> GetQuizWithQuestionsAsync(int id);
        Task<Quiz> CreateQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);
        Task AddQuestionToQuizAsync(int quizId, int questionId, int order);
        Task RemoveQuestionFromQuizAsync(int quizId, int questionId);
    }
}
