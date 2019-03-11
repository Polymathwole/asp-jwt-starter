using ASPJWTPractice.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<List<T>> ListAll();
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        //Task<T> GetSingleBySpec(ISpecification<T> spec);
        //Task<List<T>> List(ISpecification<T> spec);
    }
}
