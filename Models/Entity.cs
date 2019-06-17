using Newtonsoft.Json;

namespace Directline.Models
{
    public class Entity
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}