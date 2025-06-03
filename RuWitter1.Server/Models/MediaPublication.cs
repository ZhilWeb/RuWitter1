using System.ComponentModel.DataAnnotations;

namespace RuWitter1.Server.Models;

public class MediaPublication
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DefaultUser User { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Body { get; set; } = string.Empty;

    public DateTime PublicDate { get; set; } = DateTime.UtcNow;

    public ICollection<MediaFile>? MediaFiles { get; set; }
}
