using System;

namespace Quizziz.Models
{
    public class Answer : Base
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}