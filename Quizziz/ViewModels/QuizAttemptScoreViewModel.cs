using System;

namespace Quizziz.ViewModels
{
    public class QuizAttemptScoreViewModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
