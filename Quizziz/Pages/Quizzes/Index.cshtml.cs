using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quizziz.Services;
using Quizziz.Models;
using Quizziz.Data;
using Quizziz.Interfaces;


namespace Quizziz.Pages.Quizzes
{
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly AppDbContext _context;
        public IndexModel(IQuizService quizService, AppDbContext context)
        {
            _quizService = quizService;
            _context = context;
        }

        public IEnumerable<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public async Task OnGetAsync()
        {
            Quizzes = await _context.Quizzes.Include(q => q.QuizQuestions)
                .ToListAsync();
        }
    }
}
