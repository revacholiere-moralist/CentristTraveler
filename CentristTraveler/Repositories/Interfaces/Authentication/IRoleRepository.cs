using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRoleById(int id);
        Task<int> Create(Role role);
        Task<bool> Update(Role role);
        Task<bool> Delete(int id);

    }
}
