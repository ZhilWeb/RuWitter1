using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuWitter1.Server.Models;

public class Post : MediaPublication
{
    public ICollection<Comment>? Comments { get; set; }
}
