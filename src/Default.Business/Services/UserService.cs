using Default.Business.Interfaces;
using Default.Business.Interfaces.Repositories;
using Default.Business.Interfaces.Services;
using Default.Business.Models;
using Default.Business.Models.Validations;

namespace Default.Business.Services
{
    public class UserService : Service<User, UserValidation>, IUserService
    {
        public UserService(IUserRepository userRepository, INotifier notifier) 
            : base(userRepository, notifier)
        {
        }
    }
}
