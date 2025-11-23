namespace FreeExam.Application.Contracts.DTOs.Option
{
    public class CreateOptionDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}
