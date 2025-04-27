using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Interfaces;
using Quizziz.ViewModels;

namespace Quizziz.Pages.Quizzes
{
    public class ReviewModel : PageModel
    {
        private readonly IQuizAttemptService _quizAttemptService;
        public ReviewModel(IQuizAttemptService quizAttemptService)
        {
            _quizAttemptService = quizAttemptService;
        }

        public QuizAttemptViewModel QuizAttempt { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var attempt = await _quizAttemptService.GetQuizAttemptAsync(id);
            if (attempt == null)
            {
                return NotFound();
            }
            QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(attempt);
            return Page();
        }

    }
}
