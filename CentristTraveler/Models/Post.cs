using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Models
{
    public class Post : BaseModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ThumbnailPath { get; set; }
        public string BannerPath { get; set; }
        public string BannerText { get; set; }
        public string PreviewText { get; set; }
        public int CategoryId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
