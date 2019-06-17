using Newtonsoft.Json;

namespace Directline.Models
{
    public class BotState
    {
        [JsonProperty("eTag")]
        public string ETag { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}