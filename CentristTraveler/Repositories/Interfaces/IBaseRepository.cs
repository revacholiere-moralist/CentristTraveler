using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        void CreateTransaction(IDbTransaction transaction);
    }
}
