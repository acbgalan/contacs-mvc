using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.DataAccess.Repository.Contracts
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteASync(T entity);
        Task<bool> ExitsASync(int id);
        Task<int> SaveAsync();
    }
}
