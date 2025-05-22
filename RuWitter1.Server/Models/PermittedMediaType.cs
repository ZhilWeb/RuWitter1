using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class PermittedMediaType
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public ICollection<MediaExtension>? PermittedExtensions { get; set; }

    /*
    [JsonIgnore]
    public ICollection<MediaFile>? MediaFiles { get; set; }
    */
}