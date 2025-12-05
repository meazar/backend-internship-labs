using System.Linq.Expressions;

namespace LMS.Core.Interfaces.Repositories;

public interface IGenericRepository<T> : IRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetPagedAsync(
        int pgNo,
        int pgSize,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includes
    );

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    );

    Task<T?> SingleOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    );
}
