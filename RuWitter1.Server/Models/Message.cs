using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Message : MediaPublication
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public DefaultUser? User { get; set; }

    public int? ChatId { get; set; }

    [Required]
    [JsonIgnore]
    public Chat? Chat { get; set; }

    public bool IsReaded { get; set; } = false;

}
