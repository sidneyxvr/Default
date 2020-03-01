using Default.Business.Interfaces.Repositories;
using Default.Business.Models;
using Default.Data.Context;
using SGEM.Data.Repositories;

namespace Default.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DefaultContext context) : base(context)
        {
        }
    }
}
