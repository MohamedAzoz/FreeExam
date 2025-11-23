using FreeExam.Application.Contracts.DTOs.Option;

namespace FreeExam.Application.Contracts.DTOs.Quetion
{
    public class CreateQuestionDto
    {
        public string Content { get; set; }
        public string Type { get; set; }
        public int ExamId { get; set; }
        public List<CreateOptionDto> Options { get; set; }
    }
}
