using CentristTraveler.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dto
{
    public class PostDto : BaseDto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        [JsonProperty(PropertyName = "thumbnail_path")]
        public string ThumbnailPath { get; set; }
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }
    }
}
