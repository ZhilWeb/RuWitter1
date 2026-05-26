using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models
{
    public class CommunityPostsLikes
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CommunityId { get; set; }

        [JsonIgnore]
        public Community? Community { get; set; }

        public int PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }

        public int CommentId { get; set; }

        [JsonIgnore]
        public Comment? Comment { get; set; }
    }
}
