using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Ideastrike.Nancy.Models.Repositories
{
    public abstract class GenericRepository<C, T> :
        IGenericRepository<T>
        where T : class
        where C : DbContext
    {

        private C _context;
        public C Context { get { return _context; } }

        public GenericRepository(C ctx) {
            _context = ctx;
        }

        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public virtual T Get(int id)
        {
            var query = Context.Set<T>().Find(id);
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = Context.Set<T>().Where(predicate);
            return query;
        }

        public IQueryable<T> Include(string include)
        {
            IQueryable<T> query = Context.Set<T>().Include(include);
            return query;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entity = Get(id);
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = System.Data.EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}