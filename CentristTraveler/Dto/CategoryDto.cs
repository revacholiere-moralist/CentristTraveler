﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dto
{
    public class CategoryDto : BaseDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
    }
}
