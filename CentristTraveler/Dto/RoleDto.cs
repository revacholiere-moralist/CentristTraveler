using CentristTraveler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dto
{
    public class RoleDto
    {
        [JsonProperty(PropertyName = "role_id")]
        public int RoleId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
