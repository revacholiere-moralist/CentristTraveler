using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dto
{
    public class BaseDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "created_date")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty(PropertyName = "updated_date")]
        public DateTime UpdatedDate { get; set; }
        [JsonProperty(PropertyName = "updated_by")]
        public string UpdatedBy { get; set; }
    }
}
