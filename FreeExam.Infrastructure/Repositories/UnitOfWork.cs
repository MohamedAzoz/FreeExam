using FreeExam.Application.Contracts.Services;
using FreeExam.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace FreeExam.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
            Subjects = new GenericRepository<Subject>(context);
            Questions = new QuestionRepository(context);
            Options = new GenericRepository<Option>(context);
            Answers = new GenericRepository<Answer>(context);
             Exams = new GenericRepository<Exam>(context);
        }
        public IRepository<Subject> Subjects { get; private set; }

        public IQuestionRepository Questions { get; private set; }

        public IRepository<Option> Options { get; private set; }

        public IRepository<Answer> Answers { get; private set; }

        public IRepository<Exam> Exams { get; private set; }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
