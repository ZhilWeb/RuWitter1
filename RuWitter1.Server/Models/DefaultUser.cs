using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace RuWitter1.Server.Models;

public class DefaultUser : IdentityUser
{
    [JsonIgnore]
    public int? AvatarId { get; set; }

    [PersonalData]
    public MediaFile? Avatar { get; set; }

    [PersonalData]
    public string Nickname { get; set; } = String.Empty;

    [PersonalData]
    public short Age { get; set; } = 0;

    [PersonalData]
    [MaxLength(10000)]
    public string BriefInformation { get; set; } = String.Empty;

    [PersonalData]
    [MaxLength(300)]
    public string City { get; set; } = String.Empty;

    [PersonalData]
    [MaxLength(1000)]
    public string Interests { get; set; } = String.Empty;

    [PersonalData]
    public ICollection<Post>? Posts { get; set; }

    [PersonalData]
    public ICollection<Comment>? Comments { get; set; }

    [PersonalData]
    public ICollection<Chat>? Chats { get; set; }
}
