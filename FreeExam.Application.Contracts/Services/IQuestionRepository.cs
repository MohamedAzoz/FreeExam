using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Domain.Entities;
using System.Linq.Expressions;

namespace FreeExam.Application.Contracts.Services
{
    public interface IQuestionRepository:IRepository<Question>
    {
        public Task<Result<ICollection<Question>>> GetTestWithIncludeAsync(int examId,
            int NumberOfQuestion= 10
             , params Expression<Func<Question, object>>[] expression1);
    }
}
