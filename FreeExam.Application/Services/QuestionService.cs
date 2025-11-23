using AutoMapper;
using FreeExam.Application.Contracts;
using FreeExam.Application.Contracts.DTOs.Option;
using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Application.Contracts.DTOs.Subject;
using FreeExam.Application.Contracts.Services;
using FreeExam.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace FreeExam.Application.Services
{

    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public QuestionService(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        //public async Task<Result> AddRangeAsync(List<CreateQuestionDto> values)
        //{
        //    if (values.Count == 0)
        //    {
        //        return Result.Failure("Not found Element", 404);
        //    }
        //    var questions = mapper.Map<List<Question>>(values);
        //    var result = await unitOfWork.Questions.AddRangeAsync(questions);
        //    if (!result.IsSuccess)
        //    {
        //        return Result.Failure(result.Message, result.StatusCode ?? 500);
        //    }
        //    await unitOfWork.CommitAsync();
        //    for (int i = 0; i < questions.Count; i++)
        //    {
        //        var question = questions[i];
        //        string correctOptionText= values[i].Options.Find(x=>x.IsCorrect)!.Text;
        //        var CorrectOption = question.Options!.FirstOrDefault(x => x.Text == correctOptionText); 
        //        question.Answer= new Answer
        //        {
        //            Option = CorrectOption!
        //        };
        //        var updateResult = unitOfWork.Questions.Update(questions[i]);
        //        if (!updateResult.IsSuccess)
        //        {
        //            return Result.Failure(updateResult.Message, updateResult.StatusCode ?? 500);
        //        }
        //    }
        //    await unitOfWork.CommitAsync();

        //    return Result.Success();
        //}
        public async Task<Result> AddRangeAsync(List<CreateQuestionDto> values)
        {
            if (values == null || values.Count == 0)
            {
                return Result.Failure("Input list of questions is empty or null.", 400);
            }

            var questions = mapper.Map<List<Question>>(values);

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    foreach (var q in questions)
                    {
                        q.Answer = null;
                    }

                    var addResult = await unitOfWork.Questions.AddRangeAsync(questions);

                    if (!addResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return Result.Failure(addResult.Message, addResult.StatusCode ?? 500);
                    }

                    await unitOfWork.CommitAsync();
                    List<Answer> answersToSave = new List<Answer>();

                    for (int i = 0; i < questions.Count; i++)
                    {
                        var questionDto = values[i];
                        var questionEntity = questions[i]; 

                        var correctIndexInDto = questionDto.Options.FindIndex(x => x.IsCorrect);

                        if (correctIndexInDto == -1)
                        {
                            await transaction.RollbackAsync();
                            return Result.Failure($"Question at index {i} has no correct option.", 400);
                        }


                        var correctOptionEntity = questionEntity.Options[correctIndexInDto];

                        answersToSave.Add(new Answer
                        {
                            QuestionId = questionEntity.Id,  
                            OptionId = correctOptionEntity.Id 
                        });
                    }

                    await unitOfWork.Answers.AddRangeAsync(answersToSave);

                    await unitOfWork.CommitAsync();

                    await transaction.CommitAsync();

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Result.Failure($"An error occurred while adding questions: {ex.Message}", 500);
                }
            }
        }


        public async Task<Result> ClearAsync()
        {
            var result = await unitOfWork.Questions.ClearAsync();
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, 500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }
        /*
         function CreateAsync i woud like to create a question 
                with its options and answer by transaction way
         */
       
        public async Task<Result> CreateAsync(CreateQuestionDto questionDto)
        {
            if (questionDto == null)
            {
                return Result.Failure("Subject data is null", 400);
            }
            var question = mapper.Map<Question>(questionDto);

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                   question.Answer = null;

                    var addResult = await unitOfWork.Questions.CreateAsync(question);

                    if (!addResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return Result.Failure(addResult.Message, addResult.StatusCode ?? 500);
                    }

                    await unitOfWork.CommitAsync();

                    var correctIndexInDto = questionDto.Options.FindIndex(x => x.IsCorrect);

                    if (correctIndexInDto == -1)
                    {
                        await transaction.RollbackAsync();
                        return Result.Failure($"Question at index has no correct option.", 400);
                    }

                    var correctOptionEntity = question.Options[correctIndexInDto];

                    Answer answer = new Answer
                    {
                            QuestionId = question.Id,
                            OptionId = correctOptionEntity.Id
                    };

                    await unitOfWork.Answers.CreateAsync(answer);
                    await unitOfWork.CommitAsync();
                    await transaction.CommitAsync();

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Result.Failure($"An error occurred while adding question: {ex.Message}", 500);
                }
            }
        }

        public async Task<Result> Delete(int id)
        {
            var resultQuestion = await unitOfWork.Questions.GetByIdAsync(id);
            if (!resultQuestion.IsSuccess)
            {
                return Result.Failure("Subject not found", 404);
            }
            var result = unitOfWork.Questions.Delete(resultQuestion.Data);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, 500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result<ICollection<QuestionDto>>> FindAllAsync(
                    Expression<Func<Question, bool>> expression)
        {
            var questions =await unitOfWork.Questions.FindAllWithIncludeAsync(expression,
                x=>x.Exam,s=>s.Options,a=>a.Answer);
            if (!questions.IsSuccess)
            {
                return Result<ICollection<QuestionDto>>.Failure(questions.Message, 404);
            }
            var questionDtos = questions.Data
                     .Select(q => mapper.Map<QuestionDto>(q, opt =>
                     {
                         opt.Items["AnswerOptionId"] = q.Answer.OptionId;
                     }))
                     .ToList();
            //var questionDtos = mapper.Map<ICollection<QuestionDto>>(questions.Data);


            return Result<ICollection<QuestionDto>>.Success(questionDtos);
        }

        public async Task<Result<QuestionDto>> GetByIdAsync(int id)
        {
            var result = await unitOfWork.Questions.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return Result<QuestionDto>.Failure(result.Message, 404);
            }
            var questionDto = mapper.Map<QuestionDto>(result.Data);
            return Result<QuestionDto>.Success(questionDto);
        }

        public async Task<Result<ICollection<QuestionDto>>> GetTestQuestionsAsync
            (int examId, int NumberOfQuestions=10)
        {
            var questions = await unitOfWork.Questions.GetTestWithIncludeAsync(examId,
              NumberOfQuestions, x => x.Exam, s => s.Options, a => a.Answer);
            if (!questions.IsSuccess)
            {
                return Result<ICollection<QuestionDto>>.Failure(questions.Message, 404);
            }
            var questionDtos = questions.Data
                     .Select(q => mapper.Map<QuestionDto>(q, opt =>
                     {
                         opt.Items["AnswerOptionId"] = q.Answer.OptionId;
                     }))
                     .ToList();


            return Result<ICollection<QuestionDto>>.Success(questionDtos);
        }




    }
}
