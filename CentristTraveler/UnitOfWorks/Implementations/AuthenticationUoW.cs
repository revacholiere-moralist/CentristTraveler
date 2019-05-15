using CentristTraveler.Helper;
using CentristTraveler.Repositories.Implementations;
using CentristTraveler.Repositories.Interfaces;
using CentristTraveler.UnitOfWorks.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Implementations
{
    public class AuthenticationUoW : BaseUoW, IAuthenticationUoW
    {
        #region Private Members
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserRoleRepository _userRoleRepository;
        #endregion

        #region Constructor
        public AuthenticationUoW(IOptions<ConnectionStrings> connectionStrings,
                        IUserRepository userRepository,
                        IRoleRepository roleRepository,
                        IUserRoleRepository userRoleRepository) : base(connectionStrings)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;

        }
        #endregion

        public IUserRepository UserRepository
        {
            get { return _userRepository; }
        }
        public IRoleRepository RoleRepository
        {
            get { return _roleRepository; }
        }
        public IUserRoleRepository UserRoleRepository
        {
            get { return _userRoleRepository; }
        }
        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _userRepository.CreateTransaction(_transaction);
            _roleRepository.CreateTransaction(_transaction);
            _userRoleRepository.CreateTransaction(_transaction);
        }
    }
}
