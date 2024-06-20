//using Domain.Common;
//using Domain.Interfaces;
//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace Infrastructure.Repositories;

//public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
//{
//    protected readonly ApplicationDbContext _context;
//    protected readonly DbSet<T> _dbSet;

//    protected BaseRepository(ApplicationDbContext context)
//    {
//        _context = context;
//        _dbSet = _context.Set<T>();
//    }

//    private IQueryable<T> GetAll => _dbSet.Cast<T>();
//    private IQueryable<T> GetAllWithInclude(Expression<Func<T, bool>>[] includeProperties)
//        => includeProperties.Aggregate(GetAll, (currentEntity, includeProperty) => currentEntity.Include(includeProperty));



//}
