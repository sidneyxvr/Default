using Default.Business.Models;
using Default.Business.Models.Validations;

namespace Default.Business.Interfaces.Services
{
    public interface IUserService : IService<User, UserValidation>
    {
    }
}
