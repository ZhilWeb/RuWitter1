using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace RuWitter1.Server.Models;

public class PostContext : IdentityDbContext<DefaultUser>
{
    public PostContext(DbContextOptions<PostContext> options) : base(options)
        { }

    public DbSet<MediaExtension> MediaExtensions { get; set; } = null!;
    public DbSet<PermittedMediaType> PermittedMediaTypes { get; set; } = null!;
    public DbSet<MediaFile> MediaFiles { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
}
