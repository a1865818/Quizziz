using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Models;
using Quizziz.Interfaces;
using Quizziz.ViewModels;

namespace Quizziz.Pages.Scores
{
    public class IndexModel : PageModel
    {
        private readonly IQuizAttemptService _quizAttemptService;
        private readonly IQuizService _quizService;

        public IndexModel(IQuizAttemptService quizAttemptService, IQuizService quizService)
        {
            _quizAttemptService = quizAttemptService;
            _quizService = quizService;
        }

        public List<QuizAttemptScoreViewModel> QuizAttempts { get; set; } = new List<QuizAttemptScoreViewModel>();
        public Dictionary<int, string> QuizTitles { get; set; } = new Dictionary<int, string>();

        public async Task OnGetAsync()
        {
            var attempts = await _quizAttemptService.GetAllQuizAttemptsAsync();
            var quizzes = await _quizService.GetAllQuizzesAsync();

            // Create a dictionary of quiz titles for easy lookup
            foreach (var quiz in quizzes)
            {
                QuizTitles[quiz.Id] = quiz.Title;
            }

            // Transform quiz attempts to view models
            foreach (var attempt in attempts)
            {
                if (attempt.CompletedAt.HasValue) // Only include completed attempts
                {
                    QuizAttempts.Add(new QuizAttemptScoreViewModel
                    {
                        Id = attempt.Id,
                        QuizId = attempt.QuizId,
                        Score = attempt.Score ?? 0,
                        CompletedAt = attempt.CompletedAt.Value,
                        Duration = attempt.CompletedAt.Value - attempt.StartedAt
                    });
                }
            }

            // Order by most recent first
            QuizAttempts = QuizAttempts.OrderByDescending(a => a.CompletedAt).ToList();
        }

    }
}
