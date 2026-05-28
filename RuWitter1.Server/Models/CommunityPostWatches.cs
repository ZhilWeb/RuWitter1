using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models
{
    public class CommunityPostWatches
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DefaultUserId { get; set; }

        [JsonIgnore]
        public DefaultUser? DefaultUser { get; set; }

        [Required]
        public int CommunityId { get; set; }

        [JsonIgnore]
        public Community? Community { get; set; }

        public int PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }
    }
}
