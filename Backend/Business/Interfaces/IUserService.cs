using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IUserService : ICrud<UserModel>
    {
        Task Register(UserModel user);
        Task<UserResponse> AuthenticateAsync(string username, string password);

        Task MakeTeacher(int id);
    }
}