using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizziz.Data;
using Quizziz.Models;
using Microsoft.EntityFrameworkCore;

namespace Quizziz.Pages.Questions
{
    public class DetailModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailModel(AppDbContext context)
        {
            _context = context;
        }

        public Question Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Question == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
