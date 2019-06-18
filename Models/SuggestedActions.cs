using Newtonsoft.Json;

public class SuggestedActions
{
    [JsonProperty("actions")]
    public CardAction[] actions { get; set; }
    [JsonProperty("to")]
    public string[] To { get; set; }
}
public class CardAction {
    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("text")] 
    public string Text { get; set; }
    [JsonProperty("type")]
    public string Type {get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("value")]
    public object Value { get; set; }
}