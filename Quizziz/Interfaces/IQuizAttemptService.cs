using System.Collections.Generic;
using Quizziz.Models;
using System.Threading.Tasks;

namespace Quizziz.Interfaces
{
    public interface IQuizAttemptService
    {
        Task<QuizAttempt> StartQuizAttemptAsync(int quizId);
        Task<QuizAttempt> GetQuizAttemptAsync(int attemptId);
        Task<QuizResponse> SaveQuizResponseAsync(int attemptId, int questionId, int answerId);
        Task<QuizAttempt> CompleteQuizAttemptAsync(int attemptId);
        Task<IEnumerable<QuizAttempt>> GetAllQuizAttemptsAsync();
    }
}
