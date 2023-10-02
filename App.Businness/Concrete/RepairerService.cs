using App.Business.Abstract;
using App.Entities.DbCon;
using App.Entities.Entity;
using System.Linq.Expressions;

namespace App.Business.Concrete;

public class RepairerService : IRepairerService
{
    public IRepairerDbContext Context { get; }

    public RepairerService()
    {
        Context = new IRepairerDbContext();
    }

    public void Add(Repairer entity)
    {
        Context.Add(entity);
        Context.SaveChanges();
    }

    public void Delete(Repairer entity)
    {
        Context.Remove(entity);
        Context.SaveChanges();
    }

    public Repairer Get(Expression<Func<Repairer, bool>> filter = null!) => Context.Set<Repairer>().FirstOrDefault(filter)!;
    public IEnumerable<Repairer> GetList(Expression<Func<Repairer, bool>> filter = null!) =>
        filter == null ? Context.Set<Repairer>().ToList() : Context.Set<Repairer>().Where(filter).ToList();
    public bool Update(Repairer entity)
    {
        if (Context.Repairer?.FirstOrDefault(_ => _.Id == entity.Id && _.Name == entity.Name) is Repairer t)
        {
            Context.Update(t);
            Context.SaveChanges();
            
            return true;
        }

        return false;
    }
}
