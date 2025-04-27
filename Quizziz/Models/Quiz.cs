using System.ComponentModel.DataAnnotations;

namespace Quizziz.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Quiz Title")]
        public required string Title { get; set; }

        [Display(Name = "Description")]
        public required string Description { get; set; }

        [Display(Name = "Time Limit (minutes)")]
        public int? TimeLimit { get; set; }

        public bool RandomizeQuestions { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}
