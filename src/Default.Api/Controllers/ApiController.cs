using AutoMapper;
using Default.Api.ViewModels;
using Default.Business.Interfaces;
using Default.Business.Interfaces.Services;
using Default.Business.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Default.Api.Controllers
{
    [ApiController]
    public abstract class ApiController<TViewModel, TModel, TValidation> : ControllerBase 
        where TViewModel : ViewModel 
        where TValidation : AbstractValidator<TModel>
        where TModel : Entity
    {
        private readonly INotifier _notifier;
        protected readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IService<TModel, TValidation> _service; 

        protected ApiController(IService<TModel, TValidation> service, INotifier notifier, IMapper mapper, ILogger<ApiController<TViewModel, TModel, TValidation>> logger)
        {
            _service = service;
            _notifier = notifier;
            _mapper = mapper;
            _logger = logger;

        }

        protected bool IsValid()
        {
            return !_notifier.HasNotifications();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            _logger.LogInformation("Call custom response");
            if (!IsValid())
            {
                _logger.LogInformation("Operation is invalid");
                return BadRequest(_notifier.GetNotifications());
            }

            return Ok(result);
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            _logger.LogInformation("Call custom response");
            if (!modelState.IsValid)
            {
                _logger.LogInformation("ModelState is invalid");
                NotificarErroModelInvalida(modelState);
            }

            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            _logger.LogInformation(string.Join(' ', errors.Select(e => e.ErrorMessage)));
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message)
        {
            _logger.LogInformation($"Notify error: {message}");
            _notifier.Handle(message);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(TViewModel viewModel)
        {
            _logger.LogInformation($"Called {nameof(Post)}");
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _service.AddAsync(_mapper.Map<TModel>(viewModel));

            return CustomResponse();
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put(TViewModel viewModel)
        {
            _logger.LogInformation($"Called {nameof(Put)}");
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _service.UpdateAsync(_mapper.Map<TModel>(viewModel));

            return CustomResponse();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Called {nameof(GetById)}");
            return CustomResponse(_mapper.Map<TViewModel>(await _service.GetByIdAsync(id)));
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll(int page = 0, int pageSize = 10, 
            string search = "", string filter = "", string order = "")
        {
            _logger.LogInformation($"Called {nameof(GetAll)}");
            var data = await _service.GetPagedAsync(page, pageSize, search, filter, order);
            return CustomResponse(new PagedListViewModel<TViewModel> 
            { 
                Collection = _mapper.Map<List<TViewModel>>(data.Collection),
                Amount = data.Amount
            });
        }
    }
}