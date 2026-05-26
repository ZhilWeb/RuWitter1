using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Comment : MediaPublication
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public DefaultUser? User { get; set; }

    public int PostId { get; set; }

    [Required]
    [JsonIgnore]
    public Post? Post {  get; set; }

    [JsonIgnore]
    public ICollection<PostsLikes>? PostsLikes { get; set; }


    [JsonIgnore]
    public ICollection<CommunityPostsLikes>? CommunityPostsLikes { get; set; }
}
