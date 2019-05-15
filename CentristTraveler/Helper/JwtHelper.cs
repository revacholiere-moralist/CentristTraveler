using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CentristTraveler.Helper
{
    public class JwtHelper
    {
        private string _securityKey;
        private string _issuer;
        private string _audience;
        
        public JwtHelper(string securityKey,
                            string issuer,
                            string audience)
        {
            _securityKey = securityKey;
            _issuer = issuer;
            _audience = audience;
        }
        public string CreateToken()
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddDays(14),
                    signingCredentials: signinCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
            
    }
}
