using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Interfaces
{
    public interface IBaseUoW 
    {
        void Commit();
        void Dispose();
    }
}
