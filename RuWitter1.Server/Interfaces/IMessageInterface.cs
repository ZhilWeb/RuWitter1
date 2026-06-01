using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IMessageInterface
{
    List<Message>? GetAllMessageByChat(string userId, int chatId);
    Message? GetMessageById(Chat chat, int messageId);
    Task CreateMessage(DefaultUser hostUser, int chatId, string body, List<IFormFile> files);
    Task UpdateChat(Chat chat);
    Task DeleteChat(string userId, int chatId);

    Task<IEnumerable<Message>?> GetAllMessageByChatSearch(int chatId, string bodySubStr);


    Task<bool> DeleteMessage(string userId, int messageId);
}
