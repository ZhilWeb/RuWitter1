using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Post : MediaPublication
{
    [Required]
    public string? UserId { get; set; }

    [Required]
    public DefaultUser? User { get; set; }

    [Required]
    public int? CommunityId { get; set; }

    [Required]
    public Community? Community { get; set; }

    public ICollection<Comment>? Comments { get; set; }

    [JsonIgnore]
    public ICollection<PostsLikes>? PostsLikes { get; set; }

    [JsonIgnore]
    public ICollection<CommunityPostsLikes>? CommunityPostsLikes { get; set; }

    [JsonIgnore]
    public ICollection<CommunityPostWatches>? CommunityPostWatches { get; set; }
}
