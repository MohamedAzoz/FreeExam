using AutoMapper;
using FreeExam.Application.Contracts;
using FreeExam.Application.Contracts.DTOs.Exam;
using FreeExam.Application.Contracts.DTOs.Subject;
using FreeExam.Application.Contracts.Services;
using FreeExam.Domain.Entities;
using System.Threading.Tasks;

namespace FreeExam.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SubjectService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<Result> AddRangeAsync(List<CreateSubjectDto> values)
        {
            if (values.Count==0)
            {
                return Result.Failure("Not found Element",404);
            }
            var subjects=mapper.Map<List<Subject>>(values);
            var result = await unitOfWork.Subjects.AddRangeAsync(subjects);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, 500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result> CreateAsync(CreateSubjectDto subjectDto)
        {
            if (subjectDto == null) {
                return Result.Failure("Subject data is null", 400);
            }
            var subject = mapper.Map<Subject>(subjectDto);
            var result = await unitOfWork.Subjects.CreateAsync(subject);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, 500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var resultSubject = await unitOfWork.Subjects.GetByIdAsync(id);
            if (!resultSubject.IsSuccess)
            {
                return Result.Failure("Subject not found", 404);
            }
            var result = unitOfWork.Subjects.Delete(resultSubject.Data);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, 500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result<List<SubjectDto>>> GetAllAsync()
        {
            var result =await unitOfWork.Subjects.GetAllAsync();
           if (!result.IsSuccess)
            {
                return Result<List<SubjectDto>>.Failure(result.Message, 404);
            }
            var subjectDtos = mapper.Map<List<SubjectDto>>(result.Data);
            return Result<List<SubjectDto>>.Success(subjectDtos);
        }

        public async Task<Result<SubjectDto>> GetByIdAsync(int id)
        {
            var result =await unitOfWork.Subjects.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return Result<SubjectDto>.Failure(result.Message, 404);
            }
            var subjectDto = mapper.Map<SubjectDto>(result.Data);
            return Result<SubjectDto>.Success(subjectDto);
        }

       
    }
}
