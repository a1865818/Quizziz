
using System;
using System.Collections.Generic;

namespace Quizziz.Models
{
    public class Question : Base
    {
        public required string Text { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}