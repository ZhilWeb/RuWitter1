using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Message : MediaPublication
{
    public int? RepliedMessageId { get; set; }

    [JsonIgnore]
    public Message? RepliedMessage { get; set; }

    public int? ChatId { get; set; }

    [Required]
    [JsonIgnore]
    public Chat? Chat { get; set; }

    public bool IsReaded { get; set; } = false;

}
