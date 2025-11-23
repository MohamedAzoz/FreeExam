namespace FreeExam.Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
