using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class MediaPublication
{
    [Required]
    public int Id { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }

    [Required]
    public DefaultUser? User { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Body { get; set; } = string.Empty;

    public DateTime publicDate { get; set; } = DateTime.UtcNow;

    public ICollection<MediaFile>? MediaFiles { get; set; }
    
}
