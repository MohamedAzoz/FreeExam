namespace FreeExam.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }

        //public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public List<Option>? Options { get; set; }
    }
}
