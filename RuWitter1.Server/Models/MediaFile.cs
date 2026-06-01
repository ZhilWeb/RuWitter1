using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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


    public int ExtensionId { get; set; }

    [Required]
    [JsonIgnore]
    public MediaExtension? Extension { get; set; }

    [Required]
    public byte[] Data { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DefaultUser? User { get; set; }

    [JsonIgnore]
    public Post? Posts { get; set; }

    [JsonIgnore]
    public Comment? Comments { get; set; }

    public int? MessageId { get; set; }

    [ForeignKey("MessageId")]
    [JsonIgnore]
    public Message? Message { get; set; }

    

    [JsonIgnore]
    public Community? Communities { get; set; }
}
