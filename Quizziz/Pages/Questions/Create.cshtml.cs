using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.ViewModels;
using Quizziz.Services;


namespace Quizziz.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public CreateModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public QuestionViewModel QuestionVM { get; set; } = new QuestionViewModel();

        public void OnGet()
        {
            // Initialize with 4 empty answers
            for (int i = 0; i < 4; i++)
            {
                QuestionVM.Answers.Add(new AnswerViewModel());
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate at least 2 answers are provided
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
            await _questionService.CreateQuestionAsync(question);

            return RedirectToPage("./Index");
        }
    }
}
