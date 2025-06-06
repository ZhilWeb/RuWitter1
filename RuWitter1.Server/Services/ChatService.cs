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
        Chat newChat = new Chat();
        _context.Chats.Add(newChat);
        await _context.SaveChangesAsync();

        hostUser.Chats.Add(newChat);
        acceptorUser.Chats.Add(newChat);
        await _defaultUserService.UpdateUserAsync(hostUser);
        await _defaultUserService.UpdateUserAsync(acceptorUser);

        /*
        ChatDefaultUser chatHost = new ChatDefaultUser
        {
            Chat = newChat,
            UsersId = hostUserId,
        };

        ChatDefaultUser chatAcceptor = new ChatDefaultUser
        {
            Chat = newChat,
            UsersId = acceptorUserId,
        };

        await _context.ChatDefaultUsers.AddRangeAsync(chatHost, chatAcceptor);
        await _context.Chats.AddAsync(newChat);

        await _context.SaveChangesAsync();
        Console.WriteLine("Ok");
        */
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
