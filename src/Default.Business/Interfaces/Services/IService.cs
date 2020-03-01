using Default.Business.Models;
using Default.Business.Models.Views;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Default.Business.Interfaces.Services
{
    public interface IService<TEntity, TValidation> 
        where TEntity : Entity
        where TValidation : AbstractValidator<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<PagedList<TEntity>> GetPagedAsync(int page = 0, int pageSize = 10,
            string search = "", string filter = "", string order = "");
    }
}
