using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ideastrike.Nancy.Models.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(int id);
        void Add(T entity);
        void Delete(int id);
        void Edit(T entity);
        void Save();
    }
}
