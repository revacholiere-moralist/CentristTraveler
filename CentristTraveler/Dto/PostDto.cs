using CentristTraveler.Models;
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
        [JsonProperty(PropertyName = "banner_path")]
        public string BannerPath { get; set; }
        [JsonProperty(PropertyName = "banner_text")]
        public string BannerText { get; set; }
        [JsonProperty(PropertyName = "preview_text")]
        public string PreviewText { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }
        
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty(PropertyName = "author_display_name")]
        public string AuthorDisplayName { get; set; }

        [JsonProperty(PropertyName = "author_username")]
        public string AuthorUsername { get; set; }
    }
}
