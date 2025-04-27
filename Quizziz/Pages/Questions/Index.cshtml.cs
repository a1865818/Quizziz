using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Models;
using Quizziz.Services;

namespace Quizziz.Pages.Questions
{
    public class IndexModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public IndexModel(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public IEnumerable<Question> Questions { get; set; } = new List<Question>();

        public async Task OnGetAsync()
        {
            Questions = await _questionService.GetAllQuestionsAsync();
        }
    }
}
