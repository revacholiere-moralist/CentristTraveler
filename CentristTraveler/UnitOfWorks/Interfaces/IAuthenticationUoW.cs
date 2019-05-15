using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Interfaces
{
    public interface IAuthenticationUoW : IBaseUoW
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        void Begin();
    }
}
