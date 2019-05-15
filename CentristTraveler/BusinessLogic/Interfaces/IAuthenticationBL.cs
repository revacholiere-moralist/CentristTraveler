using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.BusinessLogic.Interfaces
{
    public interface IAuthenticationBL
    {
        bool Register(User user);
        string Authenticate(string login, string password);
    }
}
