using System.Collections.Generic;
using System.Threading.Tasks;
using LoginMVC.Models;

namespace LoginMVC.Services
{
    public interface IUserService
    {
         Task<List<User>> GetUserList();
         Task<bool> SaveUser(UserCreate user);
    }
}