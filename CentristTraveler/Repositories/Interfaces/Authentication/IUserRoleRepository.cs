using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IUserRoleRepository : IBaseRepository
    {
        Task<IEnumerable<string>> GetUserRoles(int userId);
        Task<bool> InsertUserRoles(int userId, List<Role> roles, User user);
        Task<bool> DeleteUserRoles(int userId, List<Role> roles);
        Task<bool> DeleteUserRoleByUserId(int userId);

    }
}
