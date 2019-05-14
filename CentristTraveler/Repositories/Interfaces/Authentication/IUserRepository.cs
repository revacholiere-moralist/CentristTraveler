using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByUsername(string username);

        User GetUserByEmail(string email);
        User GetUserByLoginAndPassword(string login, string password);
        int Create(User user);
        bool Update(User user);
        bool Delete(int id);

    }
}
