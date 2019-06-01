using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Models
{
    public class Tag : BaseModel
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }
}
