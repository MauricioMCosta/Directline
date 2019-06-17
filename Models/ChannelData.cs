using Newtonsoft.Json;

namespace Directline.Models
{
    public class ChannelData
    {
        [JsonProperty("clientActivityID")]
        public string ClientActivityID { get; set; }
        
    }
}