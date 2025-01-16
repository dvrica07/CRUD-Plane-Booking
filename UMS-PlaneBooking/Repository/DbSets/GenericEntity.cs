using UMS_PlaneBooking.Repository.Entities;
using UMS_PlaneBooking.Repository.Interfaces;
using UMS_PlaneBooking.Models.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace UMS_PlaneBooking.Repository.DbSets;

public class GenericEntity<TTarget> : IGenericEntity<TTarget> where TTarget : BaseEntity
{

    private readonly ApplicationContext applicationContext;

    public GenericEntity(ApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<TTarget>> Add(TTarget entity)
    {
        try 
        {
            applicationContext.Set<TTarget>().Add(entity);
            await applicationContext.SaveChangesAsync();

            return AppResult<TTarget>.CreateSucceeded(entity, "Entity successfully added");
        }
        catch (Exception ex) 
        {
            return AppResult<TTarget>.CreateFailed(ex, "An error occured when adding entity");
        }
    }

    public async Task<AppResult<IEnumerable<TTarget>>> AddRange(IEnumerable<TTarget> entities)
    {
        try 
        {
            var arrayEntities = entities.ToArray();
            applicationContext.Set<TTarget>().AddRange(arrayEntities);
            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<TTarget>>.CreateSucceeded(arrayEntities, "Entities successfully added");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<TTarget>>.CreateFailed(ex, "An error occured when adding entities");
        }
    }

    public async Task<AppResult<IEnumerable<TTarget>>> FindAsync(Expression<Func<TTarget, bool>> expression, 
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<TTarget, object>>>? includes = null,
        Expression<Func<TTarget, object>>? orderBy = null, Expression<Func<TTarget, object>>? orderByDesc = null)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<TTarget>().Where(expression);

            if(orderByDesc is not null && orderBy is null)
            {
                query = query.OrderByDescending(orderByDesc);
            }

            // priority this one if there is orderbydesd and orderby
            if(orderBy is not null)
            {
                query = query.OrderBy(orderBy);
            }

            query = query.Skip(skipCount).Take(limitCount);

            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var results = await query.ToListAsync();
            return AppResult<IEnumerable<TTarget>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<TTarget>>.CreateFailed(ex, "An error occured when finding entities");
        }
    }

    public async Task<AppResult<TTarget>> FindFirstAsync(Expression<Func<TTarget, bool>> expression, 
        IEnumerable<Expression<Func<TTarget, object>>>? includes = null)
    {
        try
        {
            var query = applicationContext.Set<TTarget>().Where(expression); //.FirstOrDefaultAsync(expression);
            
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.FirstOrDefaultAsync();

            if(result == null)
            {
                return AppResult<TTarget>.CreateFailed(new ApplicationException("Can't find entity"), "Can't find entity");
            }

            return AppResult<TTarget>.CreateSucceeded(result, "Successfully find entity");
        }
        catch (Exception ex)
        {
            return AppResult<TTarget>.CreateFailed(ex, "An error occured when finding entity");
        }
    }

    public async Task<AppResult<IEnumerable<TTarget>>> GetAllAsync()
    {
        try
        {
            var result = await applicationContext.Set<TTarget>().ToListAsync();
            return AppResult<IEnumerable<TTarget>>.CreateSucceeded(result, "Successfully get all entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<TTarget>>.CreateFailed(ex, "An error occured when getting all entities");
        }
    }

    public async Task<AppResult<TTarget>> GetByIdAsync(int id)
    {
        try
        {
            var result = await applicationContext.Set<TTarget>().FindAsync(id);
            if (result == null) 
            {
                return AppResult<TTarget>.CreateFailed(new ApplicationException("Can't find entity by id"), "Can't find entity by id");
            }

            return AppResult<TTarget>.CreateSucceeded(result, "Sucessfully find entity by id");
        }
        catch (Exception ex)
        {
            return AppResult<TTarget>.CreateFailed(ex, "An error occured when getting the entity by id");
        }
    }

    public async Task<AppResult<int>> Count(Expression<Func<TTarget, bool>> expression)
    {
        try
        {
            var result = await applicationContext.Set<TTarget>().Where(expression).CountAsync();
            return AppResult<int>.CreateSucceeded(result, "Successfully count entities.");
        }
        catch (Exception ex)
        {
            return AppResult<int>.CreateFailed(ex, "An error occured when counting the entities.");
        }
    } 

    public async Task<AppResult<TTarget>> Remove(TTarget entity)
    {
        try
        {
            applicationContext.Set<TTarget>().Remove(entity);
            await applicationContext.SaveChangesAsync();

            return AppResult<TTarget>.CreateSucceeded(entity, "Successfully remove entity");
        }
        catch (Exception ex)
        {
            return AppResult<TTarget>.CreateFailed(ex, "An error occured when removing the entity");
        }
    }

    public async Task<AppResult<IEnumerable<TTarget>>> RemoveRange(IEnumerable<TTarget> entities)
    {
        try
        {
            applicationContext.Set<TTarget>().RemoveRange(entities);
            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<TTarget>>.CreateSucceeded(entities, "Successfully removed entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<TTarget>>.CreateFailed(ex, "An error occured when removing all entities");
        }
    }

    public async Task<AppResult<TTarget>> Update(TTarget entity)
    {
        try
        {
            applicationContext.Set<TTarget>().Update(entity);
            await applicationContext.SaveChangesAsync();

            return AppResult<TTarget>.CreateSucceeded(entity, "Successfully updated entity");
        }
        catch (Exception ex)
        {
            return AppResult<TTarget>.CreateFailed(ex, "An error occured when updating the entity");
        }
    }

    public async Task<AppResult<IEnumerable<TTarget>>> UpdateRange(IEnumerable<TTarget> entities)
    {
        try
        {
            var arrayEntities = entities.ToArray();
            applicationContext.Set<TTarget>().UpdateRange(arrayEntities);
            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<TTarget>>.CreateSucceeded(arrayEntities, "Successfully updated entity");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<TTarget>>.CreateFailed(ex, "An error occured when updating entities");
        }
    }
}