using App.Business.Abstract;
using App.Entities.DbCon;
using App.Entities.Entity;
using System.Linq.Expressions;

namespace App.Business.Concrete;

public class RatingService : IRatingService
{
	public IRepairerDbContext Context { get; }

	public RatingService()
	{
		Context = new IRepairerDbContext();
	}

	public void Add(Ratings entity)
	{
		Context.Add(entity);
		Context.SaveChanges();
	}

	public void Delete(Ratings entity)
	{
		Context.Remove(entity);
		Context.SaveChanges();
	}

	public Ratings Get(Expression<Func<Ratings, bool>> filter = null!) => Context.Set<Ratings>().FirstOrDefault(filter)!;
	public IEnumerable<Ratings> GetList(Expression<Func<Ratings, bool>> filter = null!) =>
		filter == null ? Context.Set<Ratings>().ToList() : Context.Set<Ratings>().Where(filter).ToList();

	public bool Update(Ratings entity)
	{
		Ratings rating = Context.Rating?.FirstOrDefault(_ => _.Id == entity.Id)!;
		rating.Rating = entity.Rating;

		Context.SaveChanges();
		return true;
	}
}
