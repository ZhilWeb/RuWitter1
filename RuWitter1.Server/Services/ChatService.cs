using System;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Services;

public class ChatService : IChatInterface
{
    private readonly PostContext _context;
    private readonly IDefaultUserInterface _defaultUserService;

    public ChatService(PostContext context, IDefaultUserInterface defaultUserService) 
    {  
        _context = context;
        _defaultUserService = defaultUserService;
    }

    public async Task CreateChat(DefaultUser hostUser, DefaultUser acceptorUser)
    {
        // 1. Создаем новый чат
        Chat newChat = new Chat();

        // 2. Инициализируем коллекцию пользователей чата, если она вдруг null
        if (newChat.Users == null)
        {
            newChat.Users = new List<DefaultUser>();
        }

        // 3. Привязываем пользователей к чату напрямую через сущность чата
        newChat.Users.Add(hostUser);
        newChat.Users.Add(acceptorUser);

        // 4. Добавляем чат в контекст. 
        // EF Core сам поймет, что hostUser и acceptorUser уже существуют в базе (так как у них заполнены Id),
        // и просто создаст записи в промежуточной таблице связей (например, ChatDefaultUser).
        _context.Chats.Add(newChat);

        // 5. Сохраняем изменения ОДНИМ запросом
        await _context.SaveChangesAsync();
    }

    public Task DeleteChat(string userId, int chatId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Chat>? GetAllChatsByUser(DefaultUser user)
    {
        return _context.Chats
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .Where(c => c.Users.Contains(user))
            .OrderBy(p => p.Id)
            .AsNoTracking()
            .ToList();
    }

    public Chat? GetChatById(int chatId)
    {
        if(_context.Chats == null || _context.Chats.Count() == 0) return null;

        return _context.Chats
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .SingleOrDefault(ch => ch.Id == chatId);
    }

    public async Task<Chat?> GetPrivateChatByUsersAsync(string currentUserId, string targetUserId)
    {
        if (_context.Chats == null) return null;

        // Ищем чат, в котором:
        // 1. Есть текущий пользователь (u.Id == currentUserId)
        // 2. И в этом же чате есть целевой пользователь (u.Id == targetUserId)
        return await _context.Chats
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c =>
                c.Users.Any(u => u.Id == currentUserId) &&
                c.Users.Any(u => u.Id == targetUserId)
            );
    }

    public List<string?> GetUsersIdByChat(int chatId)
    {
        Chat? chat = GetChatById(chatId);

        if(chat == null) 
        {
            throw new Exception("Chat has not found.");
        }

        var userIds = new List<string?>();
        foreach (var user in chat.Users)
        {
            userIds.Add(user.Id);
        }

        return userIds;
    }

    public Task UpdateChat(Chat chat)
    {
        throw new NotImplementedException();
    }
}
