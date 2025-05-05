
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quizziz.Models;
using Quizziz.Services;

namespace Quizziz.Pages.Questions
{
    public class DeleteModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public DeleteModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public Question Question { get; set; }

        public List<Quiz> RelatedQuizzes { get; set; }

        public bool HasRelatedQuizzes => RelatedQuizzes != null && RelatedQuizzes.Count > 0;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Question = await _questionService.GetQuestionByIdAsync(id);

            if (Question == null)
            {
                return NotFound();
            }

            // Check if this question is used in any quizzes
            RelatedQuizzes = await _questionService.GetQuizzesUsingQuestionAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Check if this question is used in any quizzes before deleting
            var relatedQuizzes = await _questionService.GetQuizzesUsingQuestionAsync(id);

            if (relatedQuizzes.Count > 0)
            {
                // If question is in use, don't delete it
                ModelState.AddModelError(string.Empty, "This question cannot be deleted because it is used in one or more quizzes. Please remove it from all quizzes first.");
                Question = await _questionService.GetQuestionByIdAsync(id);
                RelatedQuizzes = relatedQuizzes;
                return Page();
            }

            // Safe to delete
            await _questionService.DeleteQuestionAsync(id);
            return RedirectToPage("./Index");
        }
    }
}