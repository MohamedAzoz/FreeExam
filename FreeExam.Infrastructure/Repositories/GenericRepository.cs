using FreeExam.Application.Contracts;
using FreeExam.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreeExam.Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<Result<ICollection<T>>> AddRangeAsync(List<T> values)
        {
            if (values == null||values.Count==0) {
                return Result<ICollection<T>>.Failure("Values cannot be null", 400);
            }
            await context.Set<T>().AddRangeAsync(values);
            return Result<ICollection<T>>.Success(values);
        }

        public async Task<Result> ClearAsync()
        {
            var entities =await context.Set<T>().ToListAsync();
            if (entities.Count == 0)
            {
                return Result.Failure("No entities to delete", 404);
            }
            context.Set<T>().RemoveRange(entities);
            return Result.Success();
        }
        public async Task<Result> RemoveRange(List<T> values)
        {
            if (values.Count == 0)
            {
                return Result.Failure("No entities to delete", 404);
            }
            context.Set<T>().RemoveRange(values);
            return Result.Success();
        }

        public async Task<Result<T>> CreateAsync(T entity)
        {
            if (entity==null)
            {
                return Result<T>.Failure("Entity can't be Add");
            }
            await context.Set<T>().AddAsync(entity);
            return Result<T>.Success(entity);
        }

        public Result Delete(T entity)
        {
            if (entity==null) {
                return Result.Failure("Entity not found",404);
            }
            context.Set<T>().Remove(entity);
            return Result.Success();
        }

        public async Task<Result<ICollection<T>>> FindAllAsync(Expression<Func<T,bool>> expression)
        {
            var query = await context.Set<T>().Where(expression).ToListAsync();
            if (query.Count == 0)
            {
                return Result<ICollection<T>>.Failure("No entities found", 404);
            }
            return Result<ICollection<T>>.Success(query);
        }

        public async Task<Result<ICollection<T>>> FindAllWithIncludeAsync(Expression<Func<T, bool>> expression
            ,params Expression<Func<T,object>>[] expression1)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var include in expression1)
            {
                query = query.Include(include);
            }
            var result = query.Where(expression);
            var list = await result.ToListAsync();
            if (list.Count == 0)
            {
                return Result<ICollection<T>>.Failure("No entities found", 404);
            }
            return Result<ICollection<T>>.Success(list);
        }

        public async Task<Result<ICollection<T>>> GetAllAsync()
        {
            var values =await context.Set<T>().ToListAsync();
            if (values.Count==0)
            {
                return Result<ICollection<T>>.Failure("No entities found", 404);
            }
            return Result<ICollection<T>>.Success(values);
        }

        public async Task<Result<T>> GetByIdAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return Result<T>.Failure("Entity not found", 404);
            }
            return Result<T>.Success(entity);
        }

        public Result Update(T entity)
        {
            if (entity==null) {
                return Result.Failure("Entity not found",404);
            }
            context.Set<T>().Update(entity);
            return Result.Success();
        }

      
    }
}
