namespace Quizziz.Models
{
    public class QuizAttempt
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int? Score { get; set; }

        public ICollection<QuizResponse> Responses { get; set; } = new List<QuizResponse>();
    }
}
