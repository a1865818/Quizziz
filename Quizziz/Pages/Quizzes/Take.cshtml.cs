using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Services;
using Quizziz.ViewModels;
using Quizziz.Models;
using Quizziz.Interfaces;

namespace Quizziz.Pages.Quizzes
{
    public class TakeModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly IQuizAttemptService _quizAttemptService;

        public TakeModel(IQuizService quizService, IQuizAttemptService quizAttemptService)
        {
            _quizService = quizService;
            _quizAttemptService = quizAttemptService;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }
        public QuizAttemptViewModel QuizAttempt { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var attemptId = HttpContext.Session.GetInt32("CurrentQuizAttemptId");
            if (attemptId.HasValue)
            {
                var attempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);
                if (attempt != null && attempt.QuizId == id && !attempt.CompletedAt.HasValue)
                {
                    QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(attempt);
                    return Page();
                }
                else
                {
                    // Clear the session if the attempt is completed or for a different quiz
                    HttpContext.Session.Remove("CurrentQuizAttemptId");
                }
            }

            // No active attempt, show the quiz details
            Quiz = await _quizService.GetQuizWithQuestionsAsync(id);

            if (Quiz == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var quizId = Quiz.Id;

            // Start a new quiz attempt
            var attempt = await _quizAttemptService.StartQuizAttemptAsync(quizId);

            if (attempt == null)
            {
                return NotFound();
            }

            // Store the attempt ID in the session
            HttpContext.Session.SetInt32("CurrentQuizAttemptId", attempt.Id);

            return RedirectToPage("./Take", new { id = quizId });
        }

        public async Task<IActionResult> OnPostAnswerAsync(int questionId, int answerId)
        {
            var attemptId = HttpContext.Session.GetInt32("CurrentQuizAttemptId");
            if (!attemptId.HasValue)
            {
                return RedirectToPage("./Index");
            }

            // Save the answer
            await _quizAttemptService.SaveQuizResponseAsync(attemptId.Value, questionId, answerId);

            // Get the updated attempt
            var attempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);
            var viewModel = QuizAttemptViewModel.FromQuizAttempt(attempt);

            // Increment the current question index
            viewModel.CurrentQuestionIndex++;

            // If this was the last question, complete the quiz
            if (viewModel.CurrentQuestionIndex >= viewModel.Questions.Count)
            {
                await _quizAttemptService.CompleteQuizAttemptAsync(attemptId.Value);
                HttpContext.Session.Remove("CurrentQuizAttemptId");
            }

            return RedirectToPage("./Take", new { id = attempt.QuizId });
        }
    }
}
