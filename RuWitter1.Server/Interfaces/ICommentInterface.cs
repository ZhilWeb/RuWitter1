using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface ICommentInterface
{
    Task<IEnumerable<Comment>?> GetAllOfPost(int postId);
    Task<Post?> GetCommentById(int commentId);
    Task CreateComment(string userId, int postId, string body, List<IFormFile> files);
    Task CreateRepliedComment(string existingUserId, int postId, int hostCommentId, string body, List<IFormFile> files);
    Task UpdateComment(Comment comment);
    Task DeletePost(int commentId);
}
