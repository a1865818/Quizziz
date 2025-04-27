using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Services;
using Quizziz.ViewModels;
using Quizziz.Models;
using Quizziz.Interfaces;

namespace Quizziz.Pages.Quizzes
{
    public class CreateModel : PageModel
    {
        private readonly IQuizService _quizService;

        public CreateModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public QuizViewModel QuizVM { get; set; } = new QuizViewModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var quiz = new Quiz
            {
                Title = QuizVM.Title,
                Description = QuizVM.Description,
                TimeLimit = QuizVM.TimeLimit,
                RandomizeQuestions = QuizVM.RandomizeQuestions
            };

            await _quizService.CreateQuizAsync(quiz);

            return RedirectToPage("./Edit", new { id = quiz.Id });
        }
    }
}
