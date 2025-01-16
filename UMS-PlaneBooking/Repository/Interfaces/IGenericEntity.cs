using System.Linq.Expressions;
using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Models.Common;

namespace UMS_PlaneBooking.Repository.Interfaces;

public interface IGenericEntity<TTarget> where TTarget : BaseEntity 
{
    Task<AppResult<TTarget>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<TTarget>>> GetAllAsync();
    // take and skip for pagination
    Task<AppResult<IEnumerable<TTarget>>> FindAsync(Expression<Func<TTarget, bool>> expression, 
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<TTarget, object>>>? includes = null,
        Expression<Func<TTarget, object>>? orderBy = null, Expression<Func<TTarget, object>>? orderByDesc = null);
    Task<AppResult<TTarget>> FindFirstAsync(Expression<Func<TTarget, bool>> expression, 
        IEnumerable<Expression<Func<TTarget, object>>>? includes = null);
    Task<AppResult<TTarget>> Add(TTarget entity);
    Task<AppResult<IEnumerable<TTarget>>> AddRange(IEnumerable<TTarget> entities);
    Task<AppResult<TTarget>> Remove(TTarget entity);
    Task<AppResult<IEnumerable<TTarget>>> RemoveRange(IEnumerable<TTarget> entities);
    Task<AppResult<TTarget>> Update(TTarget entity);
    Task<AppResult<IEnumerable<TTarget>>> UpdateRange(IEnumerable<TTarget> entities);
    Task<AppResult<int>> Count(Expression<Func<TTarget, bool>> expression);
}