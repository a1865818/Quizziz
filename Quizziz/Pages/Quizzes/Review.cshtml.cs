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

            if (!attempt.CompletedAt.HasValue)
            {
                return RedirectToPage("./Take", new { id = attempt.QuizId });
            }

            QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(attempt);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var attempt = await _quizAttemptService.GetQuizAttemptAsync(id);
            if (attempt == null)
            {
                return NotFound();
            }
            if (attempt.CompletedAt.HasValue)
            {
                return RedirectToPage("./Review", new { id = attempt.Id });
            }
            return RedirectToPage("./Take", new { id = attempt.QuizId });
        }
    }
}
