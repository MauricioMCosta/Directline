using Newtonsoft.Json;

namespace Directline.Models
{
    public class ChannelAccount
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}