using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Quizziz.Models;
namespace Quizziz.ViewModels
{
    public class AnswerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Answer text is required")]
        [Display(Name = "Answer Text")]
        public string Text { get; set; }

        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; }
    }
    public class QuestionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question Text")]
        public string Text { get; set; }

        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();

        public static QuestionViewModel FromQuestion(Question question)
        {
            var viewModel = new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                Answers = new List<AnswerViewModel>()
            };

            if (question.Answers != null)
            {
                foreach (var answer in question.Answers)
                {
                    viewModel.Answers.Add(new AnswerViewModel
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect
                    });
                }
            }

            // Ensure we always have at least 4 answer options in the view model
            while (viewModel.Answers.Count < 4)
            {
                viewModel.Answers.Add(new AnswerViewModel());
            }

            return viewModel;
        }
        public Question ToQuestion()
        {
            var question = new Question
            {
                Id = Id,
                Text = Text,
                Answers = new List<Answer>()
            };

            foreach (var answerVM in Answers)
            {
                if (!string.IsNullOrWhiteSpace(answerVM.Text))
                {
                    question.Answers.Add(new Answer
                    {
                        Id = answerVM.Id,
                        Text = answerVM.Text,
                        IsCorrect = answerVM.IsCorrect,
                        QuestionId = Id
                    });
                }
            }
            return question;
        }
    }
}
