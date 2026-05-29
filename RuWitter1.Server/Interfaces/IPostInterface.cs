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

    Task<int> SetLike(string userId, int postId, int? commentId);

    Task<int> SetLikeByCommunity(string userId, int communityId, int postId, int? commentId);

    Task<bool> DeleteLike(string userId, int postId, int? commentId = null);

    Task<bool> DeleteLikeByCommunity(string userId, int communityId, int postId, int? commentId = null);

    Task<IEnumerable<Post>?> GetPostsByNewsFeed(string userId);

    Task<bool> DeletePostWatches(string userId);

    Task<IEnumerable<Post>?> GetPostsBySearch(string userId, string postSubString,
        string communityNameSubString, List<int> communityCategoryIds, DateTime dateTimeFrom, DateTime dateTimeTo);
}
