using FreeExam.Application.Contracts.DTOs.Option;

namespace FreeExam.Application.Contracts.DTOs.Quetion
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int ExamId { get; set; }

        // Answer
        public int OptionAswerId { get; set; }

        // Options
        public List<OptionDto>? Options { get; set; }
    }
}
