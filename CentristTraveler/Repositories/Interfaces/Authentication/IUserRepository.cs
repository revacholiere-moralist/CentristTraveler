using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);

        Task<User> GetUserByEmail(string email);
        Task<string> GetHashedPassword(string login);
        Task<User> GetUserByLogin(string login);
        Task<int> Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);

    }
}
