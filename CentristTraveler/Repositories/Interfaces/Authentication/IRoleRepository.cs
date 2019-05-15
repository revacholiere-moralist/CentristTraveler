using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository
    {
        List<Role> GetAllRoles();
        Role GetRoleById(int id);

        int Create(Role role);
        bool Update(Role role);
        bool Delete(int id);

    }
}
