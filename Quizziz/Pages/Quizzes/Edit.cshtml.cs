using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quizziz.Services;
using Quizziz.Models;
using Quizziz.Data;
using Quizziz.ViewModels;
using Quizziz.Interfaces;

namespace Quizziz.Pages.Quizzes
{
    public class EditModel : PageModel
    {
        private readonly IQuizService _quizService;
        private readonly AppDbContext _context;
        private readonly IQuestionService _questionService;

        public EditModel(IQuizService quizService, IQuestionService questionService, AppDbContext context)
        {
            _quizService = quizService;
            _questionService = questionService;
            _context = context;
        }

        [BindProperty]
        public QuizViewModel QuizVM { get; set; }

        public IEnumerable<Question> AvailableQuestions { get; set; } = new List<Question>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var quiz = await _quizService.GetQuizWithQuestionsAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            QuizVM = QuizViewModel.FromQuiz(quiz);

            // Get all questions that are not already in the quiz
            var quizQuestionIds = quiz.QuizQuestions.Select(qq => qq.QuestionId).ToList();
            AvailableQuestions = await _context.Questions
                .Where(q => !quizQuestionIds.Contains(q.Id))
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var quiz = await _quizService.GetQuizByIdAsync(QuizVM.Id);

            if (quiz == null)
            {
                return NotFound();
            }

            quiz.Title = QuizVM.Title;
            quiz.Description = QuizVM.Description;
            quiz.TimeLimit = QuizVM.TimeLimit;
            quiz.RandomizeQuestions = QuizVM.RandomizeQuestions;

            await _quizService.UpdateQuizAsync(quiz);

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAddQuestionAsync(int quizId, int questionId, int order)
        {
            await _quizService.AddQuestionToQuizAsync(quizId, questionId, order);
            return RedirectToPage("./Edit", new { id = quizId });
        }

        public async Task<IActionResult> OnPostRemoveQuestionAsync(int quizId, int questionId)
        {
            await _quizService.RemoveQuestionFromQuizAsync(quizId, questionId);
            return RedirectToPage("./Edit", new { id = quizId });
        }
    }
}
