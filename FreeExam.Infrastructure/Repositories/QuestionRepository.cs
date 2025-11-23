using FreeExam.Application.Contracts;
using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Application.Contracts.Services;
using FreeExam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace FreeExam.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext context;
        private static Random random = new Random();
        public QuestionRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        // This Function return number of Question by random way with include navigation properties
        public async Task<Result<ICollection<Question>>> GetTestWithIncludeAsync(int examId,
                 int numberOfQuestions=10,
                 params Expression<Func<Question, object>>[] includes)
        {
            if (examId == 0)
                return Result<ICollection<Question>>.Failure("No found examId", 404);

            var randomIds = await context.Questions
                 .FromSqlInterpolated
                 ($"SELECT TOP ({numberOfQuestions}) Id FROM Questions Where ExamId={examId} ORDER BY NEWID()")
                 .Select(q => q.Id)
                 .ToListAsync();

            if (randomIds.Count == 0)
                return Result<ICollection<Question>>.Failure("No entities found", 404);

            if (randomIds.Count < numberOfQuestions)
                return Result<ICollection<Question>>.Failure("Not enough questions available", 400);

            // 2) اعمل Query EF طبيعي مع Includes
            IQueryable<Question> query = context.Questions
                .Where(q => randomIds.Contains(q.Id));

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // 3) نفّذ الـ Query
            var questions = await query.ToListAsync();

            return Result<ICollection<Question>>.Success(questions);
        }

    }
}
