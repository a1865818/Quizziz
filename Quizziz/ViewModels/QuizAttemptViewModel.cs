using Quizziz.Models;

namespace Quizziz.ViewModels
{
    public class QuizAttemptViewModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public bool IsCompleted { get; set; }
        public int? Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? Duration => CompletedAt.HasValue ? CompletedAt.Value - StartedAt : null;

        public List<QuizQuestionAttemptViewModel> Questions { get; set; } = new List<QuizQuestionAttemptViewModel>();

        public int CurrentQuestionIndex { get; set; } = 0;

        public bool HasTimeLimit { get; set; }
        public int? TimeLimit { get; set; }
        public DateTime? EndTime { get; set; }

        public static QuizAttemptViewModel FromQuizAttempt(QuizAttempt attempt)
        {
            var viewModel = new QuizAttemptViewModel
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz?.Title,
                IsCompleted = attempt.CompletedAt.HasValue,
                Score = attempt.Score,
                StartedAt = attempt.StartedAt,
                CompletedAt = attempt.CompletedAt,
                HasTimeLimit = attempt.Quiz?.TimeLimit.HasValue ?? false,
                TimeLimit = attempt.Quiz?.TimeLimit,
                EndTime = (attempt.Quiz?.TimeLimit.HasValue ?? false) ? attempt.StartedAt.AddMinutes(attempt.Quiz.TimeLimit.Value) : null,
                Questions = new List<QuizQuestionAttemptViewModel>()
            };

            if (attempt.Quiz?.QuizQuestions != null)
            {
                var quizQuestions = attempt.Quiz.QuizQuestions;

                // Apply randomization if needed
                if (attempt.Quiz.RandomizeQuestions)
                {
                    quizQuestions = quizQuestions.OrderBy(q => Guid.NewGuid()).ToList();
                }
                else
                {
                    quizQuestions = quizQuestions.OrderBy(q => q.Order).ToList();
                }

                foreach (var quizQuestion in quizQuestions)
                {
                    var response = attempt.Responses?.FirstOrDefault(r => r.QuestionId == quizQuestion.QuestionId);

                    var questionViewModel = new QuizQuestionAttemptViewModel
                    {
                        QuestionId = quizQuestion.QuestionId,
                        QuestionText = quizQuestion.Question?.Text,
                        Answers = quizQuestion.Question?.Answers
                            .Select(a => new QuizAnswerAttemptViewModel
                            {
                                AnswerId = a.Id,
                                AnswerText = a.Text,
                                IsSelected = response?.SelectedAnswerId == a.Id,
                                IsCorrect = a.IsCorrect
                            })
                            .ToList() ?? new List<QuizAnswerAttemptViewModel>(),
                        SelectedAnswerId = response?.SelectedAnswerId,
                        IsAnswered = response != null,
                        IsCorrect = response?.IsCorrect ?? false
                    };

                    viewModel.Questions.Add(questionViewModel);
                }
            }

            return viewModel;
        }
    }

    public class QuizQuestionAttemptViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<QuizAnswerAttemptViewModel> Answers { get; set; } = new List<QuizAnswerAttemptViewModel>();
        public int? SelectedAnswerId { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class QuizAnswerAttemptViewModel
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }
    }
}