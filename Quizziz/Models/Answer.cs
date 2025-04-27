using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizziz.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Answer Text")]
        public string Text { get; set; }

        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
