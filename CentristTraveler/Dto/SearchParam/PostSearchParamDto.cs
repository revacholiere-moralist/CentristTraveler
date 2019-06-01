using CentristTraveler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dto
{
    public class PostSearchParamDto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }
}
