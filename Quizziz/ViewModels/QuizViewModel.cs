using Quizziz.Models;
using System.ComponentModel.DataAnnotations;


namespace Quizziz.ViewModels
{
    public class QuizViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quiz title is required")]
        [Display(Name = "Quiz Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Time Limit (minutes)")]
        [Range(0, 180, ErrorMessage = "Time limit must be between 0 and 180 minutes")]
        public int? TimeLimit { get; set; }

        [Display(Name = "Randomize Questions")]
        public bool RandomizeQuestions { get; set; }

        public List<QuizQuestionViewModel> Questions { get; set; } = new List<QuizQuestionViewModel>();

        public static QuizViewModel FromQuiz(Quiz quiz)
        {
            var viewModel = new QuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                TimeLimit = quiz.TimeLimit,
                RandomizeQuestions = quiz.RandomizeQuestions,
                Questions = new List<QuizQuestionViewModel>()
            };

            if (quiz.QuizQuestions != null)
            {
                foreach (var quizQuestion in quiz.QuizQuestions.OrderBy(qq => qq.Order))
                {
                    viewModel.Questions.Add(new QuizQuestionViewModel
                    {
                        QuestionId = quizQuestion.QuestionId,
                        QuestionText = quizQuestion.Question?.Text,
                        Order = quizQuestion.Order
                    });
                }
            }

            return viewModel;
        }
    }

    public class QuizQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int Order { get; set; }
    }
}
