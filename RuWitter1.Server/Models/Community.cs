using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models
{
    public class Community
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DefaultUserId { get; set; }

        [JsonIgnore]
        public DefaultUser? DefaultUser { get; set; }


        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public int? AvatarId { get; set; }

        [JsonIgnore]
        public MediaFile? Avatar { get; set; }

        [MaxLength(10000)]
        public string BriefInformation { get; set; }

        [Required]
        public int? CommunityCategoryId { get; set; }

        [JsonIgnore]
        public CommunityCategory? CommunityCategory { get; set; }


        [JsonIgnore]
        public ICollection<CommunityPostsLikes>? CommunityPostsLikes { get; set; }

        [JsonIgnore]
        public ICollection<CommunityPostWatches>? CommunityPostWatches { get; set; }

    }
}
