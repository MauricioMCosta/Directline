using Newtonsoft.Json;

namespace Directline.Models
{
    public class ConversationAccount:ChannelAccount
    {
        [JsonProperty("isGroup")]
        public bool IsGroup { get; set; }
    }
}