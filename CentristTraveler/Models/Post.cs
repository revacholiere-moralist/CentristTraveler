using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Models
{
    public class Post : BaseModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
