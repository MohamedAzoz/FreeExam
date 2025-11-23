namespace FreeExam.Application.Contracts.DTOs.Option
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        public string Text { get; set; }
        //public bool IsCorrect { get; set; }=false;
        public int QuestionId { get; set; }
    }
}
