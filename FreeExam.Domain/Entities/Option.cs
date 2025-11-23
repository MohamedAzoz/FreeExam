namespace FreeExam.Domain.Entities
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; }
         //public bool IsCorrect { get; set; } = false;
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

    }
}
