using CentristTraveler.Models;
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
        private string _username;
        private IEnumerable<string> _roles;
        
        public JwtHelper(string securityKey,
                            string issuer,
                            string audience,
                            string username,
                            IEnumerable<string> roles)
        {
            _securityKey = securityKey;
            _issuer = issuer;
            _audience = audience;
            _username = username;
            _roles = roles;
        }
        public string CreateToken()
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, _username));

                foreach (string role in _roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                
                var tokenOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
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
