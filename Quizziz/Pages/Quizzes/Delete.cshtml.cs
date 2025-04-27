using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Services;
using Quizziz.ViewModels;
using Quizziz.Models;
using Quizziz.Interfaces;

namespace Quizziz.Pages.Quizzes
{
    public class DeleteModel : PageModel
    {
        private readonly IQuizService _quizService;
        public DeleteModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quiz = await _quizService.GetQuizWithQuestionsAsync(id);
            if (Quiz == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _quizService.DeleteQuizAsync(Quiz.Id);
            return RedirectToPage("./Index");
        }
    }
}
