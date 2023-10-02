using App.Business.Abstract;
using App.Entities.DbCon;
using App.Entities.Entity;
using System.Linq.Expressions;

namespace App.Business.Concrete;

public class CategoryService : ICategoryService
{
    public IRepairerDbContext Context { get; }

    public CategoryService()
    {
        Context = new IRepairerDbContext();
    }

    public void Add(Category entity)
    {
        Context.Add(entity);
        Context.SaveChanges();
    }

    public void Delete(Category entity)
    {
        Context.Remove(entity);
        Context.SaveChanges();
    }

    public Category Get(Expression<Func<Category, bool>> filter = null!) => Context.Set<Category>().FirstOrDefault(filter)!;
    public IEnumerable<Category> GetList(Expression<Func<Category, bool>> filter = null!) =>
        filter == null ? Context.Set<Category>() : Context.Set<Category>().Where(filter);
    public bool Update(Category entity)
    {
        if (Context.Category?.FirstOrDefault(_ => _.Id == entity.Id && _.Name == entity.Name) is null)
        {
            Category c = Context.Category?.FirstOrDefault(_ => _.Id == entity.Id)!;
            c.Name = entity.Name;
            Context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool IsExist(Category category)
    {
        if(Get(_ => _.Name == category.Name) != null)
            return true;

        return false;
    }
}
