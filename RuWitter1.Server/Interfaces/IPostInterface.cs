using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Models;
namespace RuWitter1.Server.Interfaces;

public interface IPostInterface
{
    IEnumerable<Post>? GetAllPosts(int lastPostId);
    Task<Post?> GetPostById(int postId);
    Task CreatePost(string existingUserId, string body, List<IFormFile> files);
    Task UpdatePost(Post post);
    Task DeletePost(int postId);
    int GetCountOfPosts();
}
