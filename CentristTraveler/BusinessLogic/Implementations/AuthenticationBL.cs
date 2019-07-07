using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Models;
using CentristTraveler.UnitOfWorks.Interfaces;
using System;
using DevOne.Security.Cryptography.BCrypt;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using CentristTraveler.Helper;
using Microsoft.Extensions.Options;
using CentristTraveler.Dto;
using Newtonsoft.Json;

namespace CentristTraveler.BusinessLogic.Implementations
{
    public class AuthenticationBL : IAuthenticationBL
    {
        private IAuthenticationUoW _authenticationUoW;
        private IOptions<TokenConfig> _tokenConfig;
        public AuthenticationBL(IAuthenticationUoW authenticationUoW,
                                IOptions<TokenConfig> tokenConfig)
        {
            _authenticationUoW = authenticationUoW;
            _tokenConfig = tokenConfig;
        }
        public async Task<string> Authenticate(string login, string password)
        {
            _authenticationUoW.Begin();
            bool isAuthenticated = false;
            try
            {
                string hashedPassword = await _authenticationUoW.UserRepository.GetHashedPassword(login);
                isAuthenticated = BCryptHelper.CheckPassword(password, hashedPassword);
                
                if (isAuthenticated)
                {
                    User user = await _authenticationUoW.UserRepository.GetUserByLogin(login);
                    IEnumerable<string> roles = await _authenticationUoW.UserRoleRepository.GetUserRoles(user.UserId);
                    JwtHelper jwtHelper = new JwtHelper(_tokenConfig.Value.TokenSecurityKey,
                                                            _tokenConfig.Value.Issuer,
                                                            _tokenConfig.Value.Audience,
                                                            user.Username,
                                                            roles);
                    return JsonConvert.SerializeObject(jwtHelper.CreateToken());
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<bool> Register(UserDto userDto)
        {
            _authenticationUoW.Begin();
            try
            {
                User user = new User
                {
                    Username = userDto.Username,
                    Password = BCryptHelper.HashPassword(userDto.Password, BCryptHelper.GenerateSalt(12)),
                    Email = userDto.Email,
                    DisplayName = userDto.DisplayName,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now
                };
                
                // hardcoded for now, give writer role to user
                int newUserId = await _authenticationUoW.UserRepository.Create(user);
                List<Role> roles = new List<Role>();
                foreach (RoleDto roleDto in userDto.Roles)
                {
                    Role role = new Role
                    {
                        RoleId = roleDto.RoleId,
                        Name = roleDto.Name
                    };
                    roles.Add(role);
                }
                

                var isSuccess = await _authenticationUoW.UserRoleRepository.InsertUserRoles(newUserId, roles, user);
                if (isSuccess)
                {
                    _authenticationUoW.Commit();
                }
                else
                {
                    _authenticationUoW.Dispose();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _authenticationUoW.Dispose();
                return false;
            }
        }
    }
}
