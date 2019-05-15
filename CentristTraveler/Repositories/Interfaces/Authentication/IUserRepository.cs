using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByUsername(string username);

        User GetUserByEmail(string email);
        string GetHashedPassword(string login);
        int Create(User user);
        bool Update(User user);
        bool Delete(int id);

    }
}
