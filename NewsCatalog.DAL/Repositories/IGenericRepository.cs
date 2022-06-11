using NewsCatalog.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void CreateOrUpdate(T entity);
        void Delete(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        void Save();
    }
}
