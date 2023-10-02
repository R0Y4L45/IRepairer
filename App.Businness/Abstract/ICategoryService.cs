using App.Core.DataAccess;
using App.Entities.DbCon;
using App.Entities.Entity;

namespace App.Business.Abstract;

public interface ICategoryService : IEntityRepository<Category, IRepairerDbContext>
{
    bool IsExist(Category category);
}
