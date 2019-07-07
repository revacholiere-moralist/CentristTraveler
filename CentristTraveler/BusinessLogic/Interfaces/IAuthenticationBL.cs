using CentristTraveler.Dto;
using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.BusinessLogic.Interfaces
{
    public interface IAuthenticationBL
    {
        Task<bool> Register(UserDto userDto);
        Task<string> Authenticate(string login, string password);
    }
}
