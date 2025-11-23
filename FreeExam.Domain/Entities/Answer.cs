namespace FreeExam.Domain.Entities
{
    public class Answer
    {
        public int Id { get; set; } 
        public int OptionId { get; set; }
        public Option? Option { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
