using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Models;
namespace RuWitter1.Server.Interfaces;

public interface IPostInterface
{
    IEnumerable<Post>? GetAllPosts(int lastPostId);
    Post? GetPostById(int postId);
    Task CreatePost(string existingUserId, string body, List<IFormFile> files);
    Task UpdatePost(Post post);
    Task DeletePost(int postId);
    int GetCountOfPosts();

    Task CreatePostByCommunity(string existingUserId, int communityId, string body, List<IFormFile> files);

    Task<int> SetLike(string userId, int postId);

    Task<int> SetLikeByCommunity(string userId, int communityId, int postId);

    Task<IEnumerable<Post>?> GetPostsByNewsFeed(string userId);

    Task<bool> DeletePostWatches(string userId);
}
