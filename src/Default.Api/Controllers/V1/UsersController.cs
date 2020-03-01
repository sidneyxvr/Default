using AutoMapper;
using Default.Api.ViewModels;
using Default.Business.Interfaces;
using Default.Business.Interfaces.Services;
using Default.Business.Models;
using Default.Business.Models.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Default.Api.Controllers.V1
{
    [Route("users")]
    [ApiController]
    public class UsersController : ApiController<UserViewModel, User, UserValidation>
    {
        public UsersController(IUserService userService, INotifier notifier, IMapper mapper, ILogger<UsersController> logger) 
            : base(userService, notifier, mapper, logger)
        {
        }
    }
}