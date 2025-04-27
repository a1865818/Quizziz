namespace Quizziz.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int Order { get; set; }
    }

}
