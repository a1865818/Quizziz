using System;
using System.Collections.Generic;

namespace Quizziz.Models
{
    public class Quiz : Base
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int? TimeLimit { get; set; }
        public bool RandomizeQuestions { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}