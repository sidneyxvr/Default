using Default.Business.Models;
using Default.Business.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Default.Business.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddAsync(List<TEntity> entityList);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<PagedList<TEntity>> GetPagedAsync(int page = 0, int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<TEntity> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
