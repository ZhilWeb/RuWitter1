using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Message : MediaPublication
{
    public int? MessageId { get; set; }

    [JsonIgnore]
    public Message? RepliedMessage { get; set; }

    public int? ChatId { get; set; }

    [JsonIgnore]
    public Chat? Chat { get; set; }
}
