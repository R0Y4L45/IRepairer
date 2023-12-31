﻿using App.Business.Abstract;
using App.Entities.DbCon;
using App.Entities.Entity;
using System.Linq.Expressions;

namespace App.Business.Concrete;

public class RepairerService : IRepairerService
{
    public CustomIdentityDbContext Context { get; }

    public RepairerService() => Context = new CustomIdentityDbContext();

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
        //Repairer? t = Get(_ => _.Id == entity.Id && _.Name == entity.Name);

        //if (t != null)
        //{
        //    Context.Update(t);
        //    Context.SaveChanges();

        //    return true;
        //}

        return false;
    }
}
