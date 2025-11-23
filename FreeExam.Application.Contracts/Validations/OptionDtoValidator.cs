//using FluentValidation;
//using FreeExam.Application.Contracts.DTOs.Option;

//namespace FreeExam.Application.Contracts.Validations
//{
//    public class OptionDtoValidator : AbstractValidator<CreateOptionDto>
//    {
//        public OptionDtoValidator()
//        {
//            RuleFor(x => x.Text)
//                .NotEmpty().WithMessage("Option text must not be empty.")
//                .MaximumLength(200).WithMessage("Option text must not exceed 200 characters.");
//        }
//    }
//}
