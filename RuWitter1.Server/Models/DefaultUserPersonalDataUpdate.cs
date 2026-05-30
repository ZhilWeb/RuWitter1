using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class DefaultUserPersonalDataUpdate
{
    public int? AvatarId { get; set; }
    public string? UserId { get; set; }

    public string PhoneNumber { get; set; } = String.Empty;
    public string Nickname { get; set; } = String.Empty;
    public short Age { get; set; } = 0;

    [MaxLength(10000)]
    public string BriefInformation { get; set; } = String.Empty;

    [MaxLength(300)]
    public string City { get; set; } = String.Empty;

    [MaxLength(1000)]
    public string Interests { get; set; } = String.Empty;
}
