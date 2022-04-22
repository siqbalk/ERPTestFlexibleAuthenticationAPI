using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> Get();
        T GetById(int id);
        IEnumerable<T> GetWithCondition(Expression<Func<T, bool>> expression);
        Task Post(T entity, bool saveChanges = false);
        Task AddRange(List<T> entity);
        void Put(T entity, bool saveChanges = false);
        void UpdateRange(List<T> entity);
        void Delete(int id, bool saveChanges = false);
        void DeleteRange(IEnumerable<int> entityIDs);
        bool Any(int id);
    }
}
