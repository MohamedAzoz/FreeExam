using System.Linq.Expressions;

namespace FreeExam.Application.Contracts.Services
{
    public interface IRepository<T> where T : class
    {
        public Task<Result<T>> CreateAsync(T entity);
        public Task<Result<ICollection<T>>> AddRangeAsync(List<T> values);
        public Result Update(T entity);
        public Result Delete(T entity);
        public Task<Result> ClearAsync();
        public Task<Result> RemoveRange(List<T> values);
        public Task<Result<T>> GetByIdAsync(int id);
        public Task<Result<ICollection<T>>> GetAllAsync();
        public Task<Result<ICollection<T>>> FindAllAsync(Expression<Func<T,bool>> expression);
        public Task<Result<ICollection<T>>> FindAllWithIncludeAsync(Expression<Func<T,bool>> expression
             , params Expression<Func<T, object>>[] expression1);
       

    }
}
