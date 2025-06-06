using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IChatInterface
{
    IEnumerable<Chat>? GetAllChatsByUser(DefaultUser user);
    Chat? GetChatById(int chatId);
    Task CreateChat(DefaultUser hostUser, DefaultUser acceptorUser);
    Task UpdateChat(Chat chat);
    Task DeleteChat(string userId, int chatId);
    List<string?> GetUsersIdByChat(int chatId);
}
