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

        // Calculate the end time based on the start time and time limit
        public DateTime? EndTime
        {
            get
            {
                if (!HasTimeLimit || !TimeLimit.HasValue)
                    return null;

                return StartedAt.AddMinutes(TimeLimit.Value);
            }
        }

        //public static QuizAttemptViewModel FromQuizAttempt(QuizAttempt attempt)
        //{
        //    var viewModel = new QuizAttemptViewModel
        //    {
        //        Id = attempt.Id,
        //        QuizId = attempt.QuizId,
        //        QuizTitle = attempt.Quiz?.Title,
        //        IsCompleted = attempt.CompletedAt.HasValue,
        //        Score = attempt.Score,
        //        StartedAt = attempt.StartedAt,
        //        CompletedAt = attempt.CompletedAt,
        //        HasTimeLimit = attempt.Quiz?.TimeLimit.HasValue ?? false,
        //        TimeLimit = attempt.Quiz?.TimeLimit,
        //        Questions = new List<QuizQuestionAttemptViewModel>()
        //    };

        //    if (attempt.Quiz?.QuizQuestions != null)
        //    {
        //        var quizQuestions = attempt.Quiz.QuizQuestions;

        //        // Apply randomization if needed
        //        if (attempt.Quiz.RandomizeQuestions)
        //        {
        //            quizQuestions = quizQuestions.OrderBy(q => Guid.NewGuid()).ToList();
        //        }
        //        else
        //        {
        //            quizQuestions = quizQuestions.OrderBy(q => q.Order).ToList();
        //        }

        //        foreach (var quizQuestion in quizQuestions)
        //        {
        //            var response = attempt.Responses?.FirstOrDefault(r => r.QuestionId == quizQuestion.QuestionId);

        //            var questionViewModel = new QuizQuestionAttemptViewModel
        //            {
        //                QuestionId = quizQuestion.QuestionId,
        //                QuestionText = quizQuestion.Question?.Text,
        //                Answers = quizQuestion.Question?.Answers
        //                    .Select(a => new QuizAnswerAttemptViewModel
        //                    {
        //                        AnswerId = a.Id,
        //                        AnswerText = a.Text,
        //                        IsSelected = response?.SelectedAnswerId == a.Id,
        //                        IsCorrect = a.IsCorrect  // Make sure this is being set correctly
        //                    })
        //                    .ToList() ?? new List<QuizAnswerAttemptViewModel>(),
        //                SelectedAnswerId = response?.SelectedAnswerId,
        //                IsAnswered = response != null,
        //                IsCorrect = response?.IsCorrect ?? false  // This might need debugging as well
        //            };

        //            viewModel.Questions.Add(questionViewModel);
        //        }
        //    }

        //    return viewModel;
        //}

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
                Questions = new List<QuizQuestionAttemptViewModel>()
            };

            if (attempt.Quiz?.QuizQuestions != null)
            {
                List<QuizQuestion> orderedQuestions;

                // If we have a stored question order, use it
                if (!string.IsNullOrEmpty(attempt.QuestionOrder))
                {
                    var questionOrder = attempt.QuestionOrder.Split(',').Select(int.Parse).ToList();
                    orderedQuestions = new List<QuizQuestion>();

                    // Reconstruct the quiz questions in the stored order
                    foreach (var questionId in questionOrder)
                    {
                        var question = attempt.Quiz.QuizQuestions.FirstOrDefault(qq => qq.QuestionId == questionId);
                        if (question != null)
                        {
                            orderedQuestions.Add(question);
                        }
                    }
                }
                else
                {
                    // Fallback to old behavior if no stored order
                    orderedQuestions = attempt.Quiz.QuizQuestions.OrderBy(q => q.Order).ToList();
                }

                foreach (var quizQuestion in orderedQuestions)
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

    // Other classes remain the same
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