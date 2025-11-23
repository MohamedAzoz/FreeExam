using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Domain.Entities;
using System.Linq.Expressions;

namespace FreeExam.Application.Contracts.Services
{
    public interface IQuestionService
    {
        public Task<Result> CreateAsync(CreateQuestionDto questionDto);
        public Task<Result> AddRangeAsync(List<CreateQuestionDto> values);
        //public Result UpdateAsync(CreateQuestionDto entity);
        //public  Task<Result> AddAsync(
        //    CreateQuestionDto questionDto);
        public Task<Result<QuestionDto>> GetByIdAsync(int id);
        public Task<Result> Delete(int id);

        public Task<Result> ClearAsync();

        public Task<Result<ICollection<QuestionDto>>> GetTestQuestionsAsync(int examId, int NumberOfQuestions);
        public Task<Result<ICollection<QuestionDto>>> FindAllAsync(Expression<Func<Question, bool>> expression);

    }
}
