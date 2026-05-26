using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
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
    public DbSet<Community> Communities { get; set; } = null!;
    public DbSet<CommunityCategory> CommunityCategories { get; set; } = null!;
    public DbSet<PostsLikes> PostsLikes { get; set; } = null!;
    public DbSet<CommunityPostsLikes> CommunityPostsLikes { get; set; } = null!;
    public DbSet<CommunityPostWatches> CommunityPostWatches { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MediaExtension>()
            .HasOne(m => m.PermittedMediaType)
            .WithMany(p => p.MediaExtensions)
            .HasForeignKey(m => m.PermittedMediaTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PermittedMediaType>().HasData(
            new PermittedMediaType { Id = 1, Type = "Image" },    
            new PermittedMediaType { Id = 2, Type = "Text" }  
        );

        builder.Entity<CommunityCategory>().HasData(
            new CommunityCategory { Id = 1, Name = "Политика"},  
            new CommunityCategory { Id = 2, Name = "Общество"},  
            new CommunityCategory { Id = 3, Name = "Происшествия"},  
            new CommunityCategory { Id = 4, Name = "Экономика"},  
            new CommunityCategory { Id = 5, Name = "Наука и техника"},
            new CommunityCategory { Id = 6, Name = "Спорт" },
            new CommunityCategory { Id = 7, Name = "Культура"}
            
        );

        builder.Entity<MediaFile>()
        .HasIndex(m => m.Name)
        .IsUnique();
    }
}
