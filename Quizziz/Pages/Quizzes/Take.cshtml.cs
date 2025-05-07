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

        public async Task<IActionResult> OnGetAsync(int id, bool clear = false, int? attemptId = null)
        {
            if (clear)
            {
                // Force clear any existing session data
                HttpContext.Session.Remove("CurrentQuizAttemptId");
                HttpContext.Session.Remove("CurrentQuestionIndex");
            }

            // If attemptId is provided, show that completed attempt
            if (attemptId.HasValue)
            {
                var completedAttempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);
                if (completedAttempt != null && completedAttempt.CompletedAt.HasValue)
                {
                    QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(completedAttempt);
                    return Page();
                }
            }

            var sessionAttemptId = HttpContext.Session.GetInt32("CurrentQuizAttemptId");
            if (sessionAttemptId.HasValue)
            {
                var attempt = await _quizAttemptService.GetQuizAttemptAsync(sessionAttemptId.Value);
                if (attempt != null && attempt.QuizId == id)
                {
                    QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(attempt);

                    // Get the current question index from session if it exists
                    var currentQuestionIndex = HttpContext.Session.GetInt32("CurrentQuestionIndex");
                    if (currentQuestionIndex.HasValue)
                    {
                        QuizAttempt.CurrentQuestionIndex = currentQuestionIndex.Value;
                    }

                    // Check if time limit has expired
                    if (QuizAttempt.HasTimeLimit && QuizAttempt.EndTime.HasValue && DateTime.Now > QuizAttempt.EndTime.Value)
                    {
                        //If the time limit has expired, complete the quiz attempt
                        if (!attempt.CompletedAt.HasValue)
                        {
                            await _quizAttemptService.CompleteQuizAttemptAsync(sessionAttemptId.Value);
                            HttpContext.Session.Remove("CurrentQuizAttemptId");
                            HttpContext.Session.Remove("CurrentQuestionIndex");

                            // Reload the attempt to get completion status
                            attempt = await _quizAttemptService.GetQuizAttemptAsync(sessionAttemptId.Value);
                            QuizAttempt = QuizAttemptViewModel.FromQuizAttempt(attempt);
                        }
                    }

                    return Page();
                }
                else
                {
                    //// Clear the session if the attempt is for a different quiz
                    HttpContext.Session.Remove("CurrentQuizAttemptId");
                    HttpContext.Session.Remove("CurrentQuestionIndex");
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

            // Clear any previous attempt from the session
            HttpContext.Session.Remove("CurrentQuizAttemptId");
            HttpContext.Session.Remove("CurrentQuestionIndex");

            // Start a new quiz attempt
            var attempt = await _quizAttemptService.StartQuizAttemptAsync(quizId);

            if (attempt == null)
            {
                return NotFound();
            }

            // Store the new attempt ID in the session
            HttpContext.Session.SetInt32("CurrentQuizAttemptId", attempt.Id);

            // Initialize the current question index to 0
            HttpContext.Session.SetInt32("CurrentQuestionIndex", 0);

            return RedirectToPage("./Take", new { id = quizId });
        }

        public async Task<IActionResult> OnPostAnswerAsync(int questionId, int answerId, int currentQuestionIndex)
        {
            var attemptId = HttpContext.Session.GetInt32("CurrentQuizAttemptId");
            if (!attemptId.HasValue)
            {
                return RedirectToPage("./Index");
            }

            // Get the attempt to check if it's still active
            var attempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);
            if (attempt.CompletedAt.HasValue)
            {
                // Quiz is already completed, redirect to take page
                return RedirectToPage("./Take", new { id = attempt.QuizId });
            }

            // Save the answer
            await _quizAttemptService.SaveQuizResponseAsync(attemptId.Value, questionId, answerId);

            // Get the updated attempt
            attempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);
            var viewModel = QuizAttemptViewModel.FromQuizAttempt(attempt);

            // Use currentQuestionIndex from the form, then increment it
            viewModel.CurrentQuestionIndex = currentQuestionIndex + 1;

            // Store the updated question index in session
            HttpContext.Session.SetInt32("CurrentQuestionIndex", viewModel.CurrentQuestionIndex);

            // If this was the last question, complete the quiz
            // If this was the last question, complete the quiz
            if (viewModel.CurrentQuestionIndex >= viewModel.Questions.Count)
            {
                await _quizAttemptService.CompleteQuizAttemptAsync(attemptId.Value);

                return RedirectToPage("./Take", new { id = attempt.QuizId, attemptId = attemptId.Value });
            }

            return RedirectToPage("./Take", new { id = attempt.QuizId });
        }

        // In TakeModel.cs
        public async Task<IActionResult> OnPostTimeUpAsync()
        {
            var attemptId = HttpContext.Session.GetInt32("CurrentQuizAttemptId");
            if (!attemptId.HasValue)
            {
                return RedirectToPage("./Index");
            }

            // Get the attempt
            var attempt = await _quizAttemptService.GetQuizAttemptAsync(attemptId.Value);

            // Complete the quiz if not already completed
            if (!attempt.CompletedAt.HasValue)
            {
                await _quizAttemptService.CompleteQuizAttemptAsync(attemptId.Value);
            }

            // Clear session - Make sure BOTH values are removed
            HttpContext.Session.Remove("CurrentQuizAttemptId");
            HttpContext.Session.Remove("CurrentQuestionIndex");

            return RedirectToPage("./Review", new { id = attempt.Id });  // Change to redirect to Review
        }
    }
}