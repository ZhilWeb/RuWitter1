using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IChatInterface
{
    IEnumerable<Chat>? GetAllChatsByUser(string userId);
    Task<Chat?> GetChatById(string userId, int chatId);
    Task CreateChat(string UserId1, string UserId2);
    Task UpdateChat(Chat chat);
    Task DeleteChat(string userId, int chatId);
}
