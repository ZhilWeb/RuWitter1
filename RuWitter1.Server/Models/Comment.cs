using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Comment : MediaPublication
{

    public int PostId { get; set; }

    [Required]
    [JsonIgnore]
    public Post? Post {  get; set; }

    public int? RepliedCommentId { get; set; }

    [JsonIgnore]
    public Comment? RepliedComment { get; set; }


}
