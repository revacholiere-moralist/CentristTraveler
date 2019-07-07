using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
