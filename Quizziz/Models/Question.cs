using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizziz.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Question Text")]
        public required string Text { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
