using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.ViewModels;
using Quizziz.Services;

namespace Quizziz.Pages.Questions
{
    public class EditModel : PageModel
    {
        private readonly IQuestionService _questionService;
        public EditModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public QuestionViewModel QuestionVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            QuestionVM = QuestionViewModel.FromQuestion(question);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            int validAnswers = 0;
            bool hasCorrectAnswer = false;

            foreach (var answer in QuestionVM.Answers)
            {
                if (!string.IsNullOrWhiteSpace(answer.Text))
                {
                    validAnswers++;
                    if (answer.IsCorrect)
                    {
                        hasCorrectAnswer = true;
                    }
                }
            }
            if (validAnswers < 2)
            {
                ModelState.AddModelError(string.Empty, "At least 2 answer options are required.");
            }
            if (!hasCorrectAnswer)
            {
                ModelState.AddModelError(string.Empty, "You must mark one answer as correct.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var question = QuestionVM.ToQuestion();
            await _questionService.UpdateQuestionAsync(question);
            return RedirectToPage("./Index");
        }
    }
}
