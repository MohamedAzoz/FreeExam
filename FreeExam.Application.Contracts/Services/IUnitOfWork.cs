using FreeExam.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
namespace FreeExam.Application.Contracts.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContextTransaction BeginTransaction();

        // يمكنك أيضاً إضافة نسخة غير متزامنة (مستحسن في ASP.NET Core)
        Task<IDbContextTransaction> BeginTransactionAsync();
        public IRepository<Subject> Subjects { get; }
        public IRepository<Exam> Exams { get; }
        public IQuestionRepository Questions { get; }
        public IRepository<Option> Options { get; }
        public IRepository<Answer> Answers { get; }
        Task CommitAsync();
    }
}
