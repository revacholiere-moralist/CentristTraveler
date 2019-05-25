using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IUserRoleRepository : IBaseRepository
    {
        List<string> GetUserRoles(int userId);
        bool InsertUserRoles(int userId, List<Role> roles, User user);
        bool DeleteUserRoles(int userId, List<Role> roles);
        bool DeleteUserRoleByUserId(int userId);

    }
}
