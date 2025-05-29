using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
namespace RuWitter1.Server.Models;

public class MediaFile
{
    public int Id { get; set; }

    [Required]
    public Guid Name { get; set; }

    [Required]
    public string ContentType { get; set; }

    [JsonIgnore]
    public int ExtensionId { get; set; }

    [Required]
    [JsonIgnore]
    public MediaExtension? Extension { get; set; }

    [Required]
    public byte[] Data { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
}
