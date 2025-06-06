using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Services;

public class MessageService : IMessageInterface
{
    private readonly PostContext _context;
    // private readonly IDefaultUserInterface _defaultUserService;
    private readonly IMediaFileInterface _mediaFileService;
    private readonly IChatInterface _chatService;

    public MessageService(PostContext context, IMediaFileInterface mediaFileService, IChatInterface chatService)
    {
        _context = context;
        // _defaultUserService = defaultUserService;
        _mediaFileService = mediaFileService;
        _chatService = chatService;
    }
    public async Task CreateMessage(DefaultUser hostUser, int chatId, string body, List<IFormFile> files)
    {
        if (body == null)
        {
            throw new Exception("Body has invalid.");
        }

        Chat? existingChat = _chatService.GetChatById(chatId);
        if (existingChat == null)
        {
            throw new Exception("Chat has not found.");
        }


        if (files.Count == 0 || files == null)
        {
            // создание сообщения без файлов
            Message message = new Message
            {
                UserId = hostUser.Id,
                Body = body,
                MediaFiles = null,
                ChatId = existingChat.Id,
                RepliedMessageId = null
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            Console.WriteLine("Ok");
            return;
        }

        // получение файлов
        List<MediaFile>? postMediaFiles = new List<MediaFile>();
        foreach (var file in files)
        {
            MediaFile? uploadFile = await _mediaFileService.InitMediaFile(file);
            if (uploadFile == null)
            {
                throw new Exception("Could not init file.");
            }
            postMediaFiles.Add(uploadFile);
        }

        // создание сообщения с файлами
        Message messageWithFiles = new Message
        {
            UserId = hostUser.Id,
            Body = body,
            MediaFiles = postMediaFiles,
            ChatId = existingChat.Id,
            RepliedMessageId = null
        };

        await _context.Messages.AddAsync(messageWithFiles);
        await _context.SaveChangesAsync();
        Console.WriteLine("Ok");
        return;
    }

    public Task DeleteChat(string userId, int chatId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Message>?> GetAllMessageByChat(DefaultUser user, int chatId)
    {
        Chat? existingChat = _chatService.GetChatById(chatId);

        if (existingChat == null)
        {
            throw new Exception("Chat has not found.");
        }

        return await _context.Messages.AsNoTracking().Where(m => m.ChatId == existingChat.Id).ToListAsync();
    }

    public Message? GetMessageById(Chat chat, int messageId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateChat(Chat chat)
    {
        throw new NotImplementedException();
    }
}
