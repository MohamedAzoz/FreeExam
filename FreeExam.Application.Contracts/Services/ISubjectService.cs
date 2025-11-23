using FreeExam.Application.Contracts.DTOs.Subject;
using FreeExam.Domain.Entities;

namespace FreeExam.Application.Contracts.Services
{
    public interface ISubjectService
    {
        public Task<Result> CreateAsync(CreateSubjectDto subjectDto);
        public Task<Result> AddRangeAsync(List<CreateSubjectDto> values);

        //public Result UpdateAsync(CreateSubjectDto entity);
        public Task<Result<SubjectDto>> GetByIdAsync(int id);
        public Task<Result<List<SubjectDto>>> GetAllAsync();
        public Task<Result> DeleteAsync(int id);
    }
}
