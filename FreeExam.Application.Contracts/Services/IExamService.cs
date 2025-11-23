using FreeExam.Application.Contracts.DTOs.Exam;
using FreeExam.Domain.Entities;
using System.Linq.Expressions;

namespace FreeExam.Application.Contracts.Services
{
    public interface IExamService
    {
        public Task<Result> CreateAsync(CreateExamDto subjectDto);
        public Task<Result<ExamDto>> GetByIdAsync(int id);
        public Task<Result<List<ExamDto>>> GetAllAsync();
        public Task<Result<ICollection<ExamDto>>> FindAllAsync(Expression<Func<Exam, bool>> expression);
        public Task<Result> DeleteAsync(int id);
    }
}
