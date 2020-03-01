using Default.Business.Interfaces;
using Default.Business.Interfaces.Repositories;
using Default.Business.Interfaces.Services;
using Default.Business.Models;
using Default.Business.Models.Views;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace Default.Business.Services
{
    /// <summary>
    /// Generic service that contains main methods can be used by specialized services
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TValidation"></typeparam>
    public abstract class Service<TEntity, TValidation> : IService<TEntity, TValidation> 
        where TEntity : Entity
        where TValidation : AbstractValidator<TEntity>, new()
    {
        private readonly IRepository<TEntity> _repository;
        private readonly INotifier _notifier;

        public Service(IRepository<TEntity> repository, INotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        /// <summary>
        /// Notify errors
        /// </summary>
        /// <param name="validationResult"></param>
        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ToString());
            }
        }

        /// <summary>
        /// Notify error
        /// </summary>
        /// <param name="validationResult"></param>
        protected void Notify(string message)
        {
            _notifier.Handle(message);
        }

        /// <summary>
        /// Execute validations
        /// </summary>
        /// <param name="validation"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected bool ExecuteValidation(TValidation validation, TEntity entity)
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid)
            {
                return true;
            }

            Notify(validator);

            return false;
        }

        // <summary>
        /// Add a entity async
        /// </summary>
        /// <param name="entity"></param>
        public async Task AddAsync(TEntity entity)
        {
            if(!ExecuteValidation(new TValidation(), entity))
            {
                return;
            }

            await _repository.AddAsync(entity);
        }

        // <summary>
        /// Update a entity async
        /// </summary>
        /// <param name="entity"></param>
        public async Task UpdateAsync(TEntity entity)
        {
            if (!ExecuteValidation(new TValidation(), entity))
            {
                return;
            }

            await _repository.UpdateAsync(entity);
        }

        /// <summary>
        /// Get a register by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a register</returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get a paged list
        /// </summary>
        /// <param name="page">integer</param>
        /// <param name="pageSize">interger</param>
        /// <param name="search">a word to be search</param>
        /// <param name="filter">string with patter: field1=value1;field2<=value2</param>
        /// <param name="order">string with patter: field1 asc</param>
        /// <returns>paged list that contains a collection paged and total amount</returns>
        public async Task<PagedList<TEntity>> GetPagedAsync(int page = 0, int pageSize = 10, 
            string search = "", string filter = "", string order = "")
        {
            return await _repository.GetPagedAsync(page, pageSize, search, filter, order);
        }
    }
}
