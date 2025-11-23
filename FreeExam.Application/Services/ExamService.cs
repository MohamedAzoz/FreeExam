using AutoMapper;
using FreeExam.Application.Contracts;
using FreeExam.Application.Contracts.DTOs.Exam;
using FreeExam.Application.Contracts.Services;
using FreeExam.Domain.Entities;
using System.Linq.Expressions;

namespace FreeExam.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ExamService(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Result> CreateAsync(CreateExamDto createExamDto)
        {
            if (createExamDto == null)
            {
                return Result.Failure("Not Found", 404);
            }
            var exam=mapper.Map<Exam>(createExamDto);
            var result=await unitOfWork.Exams.CreateAsync(exam);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, result.StatusCode ??500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var exam=await unitOfWork.Exams.GetByIdAsync(id);
            if (!exam.IsSuccess)
            {
                return Result.Failure(exam.Message, (exam.StatusCode)??404);
            }
            var result1=await unitOfWork.Questions.FindAllAsync(x=>x.ExamId==id);
            if (!result1.IsSuccess)
            {
                return Result.Failure(result1.Message, (result1.StatusCode) ?? 500);
            }

           var result2=await unitOfWork.Questions.RemoveRange(result1.Data.ToList());
            if (!result2.IsSuccess)
            {
                return Result.Failure(result2.Message, (result2.StatusCode) ?? 500);
            }
            await unitOfWork.CommitAsync();
            var result = unitOfWork.Exams.Delete(exam.Data);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message, (result.StatusCode)??500);
            }
            await unitOfWork.CommitAsync();
            return Result.Success();
        }

        public async Task<Result<ICollection<ExamDto>>> FindAllAsync(Expression<Func<Exam, bool>> expression)
        {
            var result=await unitOfWork.Exams.FindAllAsync(expression);
            if (!result.IsSuccess)
            {
                return Result<ICollection<ExamDto>>.Failure(result.Message, (result.StatusCode)??500 );
            }
            var examDtos=mapper.Map<ICollection<ExamDto>>(result.Data);
            return Result<ICollection<ExamDto>>.Success(examDtos);
        }

        public async Task<Result<List<ExamDto>>> GetAllAsync()
        {
            var result = await unitOfWork.Exams.GetAllAsync();
            if (!result.IsSuccess) {
                return Result<List<ExamDto>>.Failure(result.Message, (result.StatusCode) ?? 500);
            }
            var examDtos = mapper.Map<List<ExamDto>>(result.Data);
            return Result<List<ExamDto>>.Success(examDtos);
        }

        public async Task<Result<ExamDto>> GetByIdAsync(int id)
        {
            var result = await unitOfWork.Exams.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return Result<ExamDto>.Failure(result.Message, (result.StatusCode) ?? 500);
            }
            var examDto = mapper.Map<ExamDto>(result.Data);
            return Result<ExamDto>.Success(examDto);
        }
    }
}
