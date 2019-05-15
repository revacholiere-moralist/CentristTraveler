﻿using CentristTraveler.BusinessLogic.Interfaces;
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
        public string Authenticate(string login, string password)
        {
            _authenticationUoW.Begin();
            bool isAuthenticated = false;
            try
            {
                string hashedPassword = _authenticationUoW.UserRepository.GetHashedPassword(login);
                isAuthenticated = BCryptHelper.CheckPassword(password, hashedPassword);
                if (isAuthenticated)
                {
                    JwtHelper jwtHelper = new JwtHelper(_tokenConfig.Value.TokenSecurityKey,
                                                            _tokenConfig.Value.Issuer,
                                                            _tokenConfig.Value.Audience);
                    return jwtHelper.CreateToken();
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

        public bool Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}