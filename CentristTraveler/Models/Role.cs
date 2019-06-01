using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Models
{
    public class Role : BaseModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}
