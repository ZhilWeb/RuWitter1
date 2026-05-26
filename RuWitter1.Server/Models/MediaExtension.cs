using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class MediaExtension
{
    public int Id { get; set; }

    [Required]
    [MaxLength(5)]
    public string Name { get; set; }

    public int PermittedMediaTypeId { get; set; }

    [JsonIgnore]
    public PermittedMediaType? PermittedMediaType { get; set; }

    [JsonIgnore]
    public ICollection<MediaFile>? MediaFiles { get; set; }


}
