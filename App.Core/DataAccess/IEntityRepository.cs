using App.Core.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Core.DataAccess;

public interface IEntityRepository<TEntity, TContext> 
    where TEntity : class, IEntity, new()
    where TContext : DbContext
{
    TContext Context { get; }
    TEntity Get(Expression<Func<TEntity, bool>> filter = null!);
    IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null!);
    void Add(TEntity entity);
    void Delete(TEntity entity);
    bool Update(TEntity entity);
}
