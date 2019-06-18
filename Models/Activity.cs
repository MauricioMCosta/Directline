using Newtonsoft.Json;
using System;

namespace Directline.Models
{
    public class Activity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("serviceUrl")]
        public string ServiceUrl { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("localTimestamp")]
        public string LocalTimestamp { get; set; }
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }
        [JsonProperty("from")]
        public ChannelAccount From { get; set; }
        [JsonProperty("conversation")]
        public ConversationAccount Conversation { get; set; }
        [JsonProperty("recipient")]
        public ChannelAccount Recipient { get; set; }
        [JsonProperty("replyToId")]
        public string ReplyToId { get; set; }
        [JsonProperty("channelData")]
        public object ChannelData { get; set; }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("textFormat")]
        public string TextFormat { get; set; }
        [JsonProperty("attachmentLayout")]
        public string AttachmentLayout { get; set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
        [JsonProperty("entities")]
        public Entity[] Entities { get; set; }
        [JsonProperty("membersAdded")]
        public ChannelAccount[] MembersAdded { get; set; }
        [JsonProperty("membersRemoved")]
        public ChannelAccount[] MembersRemoved { get; set; }
        [JsonProperty("topicName")]
        public string TopicName { get; set; }
        [JsonProperty("historyDisclosed")]
        public bool HistoryDisclosed { get; set; }
        [JsonProperty("inputHint")]
        public string InputHint{ get; set; }
        [JsonProperty("suggestedActions")]
        public SuggestedActions SuggestedActions { get; set; }

        public Activity()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow.ToString("o");
            LocalTimestamp = DateTime.Now.ToString("s");
        }
    }

    public class MessageActivity:Activity {
        public MessageActivity():base()
        {
            Type = "message";
            Locale = "en-US";
            TextFormat = "plain";
        }
    }

    public class ConversationUpdateActivity: Activity
    {
        public ConversationUpdateActivity():base()
        {
            Type = "conversationUpdate";
        }
    }
}