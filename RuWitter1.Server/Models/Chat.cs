using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Chat
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    public ICollection<DefaultUser> Users { get; set; } = new List<DefaultUser>();

    public ICollection<Message>? Messages { get; set; }
}
