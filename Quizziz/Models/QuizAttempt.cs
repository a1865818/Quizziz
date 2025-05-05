using System;
using System.Collections.Generic;

namespace Quizziz.Models
{
    public class QuizAttempt : Base
    {
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int? Score { get; set; }

        // Store the order of questions for this attempt
        public string QuestionOrder { get; set; }

        public ICollection<QuizResponse> Responses { get; set; } = new List<QuizResponse>();
    }
}