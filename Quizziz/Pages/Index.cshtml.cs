using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quizziz.Data;
using Quizziz.Services;
using Quizziz.Interfaces;
using Quizziz.Models;

namespace Quizziz.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;

        public IndexModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public IEnumerable<Quiz> LatestQuizzes { get; set; } = new List<Quiz>();

        public async Task OnGetAsync()
        {
            LatestQuizzes = await _quizService.GetAllQuizzesAsync();
        }
    }
}