﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Directline.Models
{
    public class ConversationObject
    {
        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }        
    }

    public class Conversation
    {
        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty("botEndpoint")]
        public BotData BotEndpoint { get; set; }
        [JsonProperty("history")]
        public List<Activity> History { get; set; }

        public Conversation()
        {
            History = new List<Activity>();
        }
    }
}