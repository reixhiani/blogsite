using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllAsQueryable();

        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> SaveChanges();

        Task SaveChangesAsync();
    }

}
