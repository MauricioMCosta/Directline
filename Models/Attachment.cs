using Newtonsoft.Json;

namespace Directline.Models
{
    public class Attachment
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }
        [JsonProperty("content")]
        public object Content { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }
}