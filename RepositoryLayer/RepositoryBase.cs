using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        internal readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> Get()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetWithCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual async Task Post(T entity, bool saveChanges = false)
        {
            await _context.Set<T>().AddAsync(entity);
            if (saveChanges)
                await _context.SaveChangesAsync();
        }

        public virtual async Task AddRange(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public virtual void UpdateRange(List<T> entity)
        {
            _context.Set<T>().UpdateRange(entity);
        }

        public virtual void Put(T entity, bool saveChanges = false)
        {
            _context.Set<T>().Update(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public virtual void Delete(int id, bool saveChanges = false)
        {
            _context.Set<T>().Remove(GetById(id));
            if (saveChanges)
                _context.SaveChanges();
        }
        public virtual void DeleteRange(IEnumerable<int> entityIDs)
        {
            foreach (var id in entityIDs)
                _context.Set<T>().Remove(GetById(id));
        }

        public bool Any(int id)
        {
            return GetById(id) != null;
        }
    }
}
