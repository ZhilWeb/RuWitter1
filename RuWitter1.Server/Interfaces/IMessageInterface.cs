using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IMessageInterface
{
    Task<IEnumerable<Message>?> GetAllMessageByChat(DefaultUser user, int chatId);
    Message? GetMessageById(Chat chat, int messageId);
    Task CreateMessage(DefaultUser hostUser, int chatId, string body, List<IFormFile> files);
    Task UpdateChat(Chat chat);
    Task DeleteChat(string userId, int chatId);
}
