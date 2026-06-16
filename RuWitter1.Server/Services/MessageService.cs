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
            return ;
        }

        Chat? existingChat = _chatService.GetChatById(chatId);
        if (existingChat == null)
        {
            return ;
        }


        if (files.Count == 0 || files == null)
        {
            // создание сообщения без файлов
            Message message = new Message
            {
                UserId = hostUser.Id,
                Body = DataProtector.Encrypt(body),
                MediaFiles = null,
                ChatId = existingChat.Id
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
            Body = DataProtector.Encrypt(body),
            MediaFiles = postMediaFiles,
            ChatId = existingChat.Id
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

    public List<Message>? GetAllMessageByChat(string userId, int chatId)
    {
        Chat? existingChat = _chatService.GetChatById(chatId);

        if (existingChat == null)
        {
            return [];
        }

        var messages = _context.Messages
            .Include(m => m.MediaFiles)
            .Where(m => m.ChatId == existingChat.Id)
            .OrderByDescending(m => m.PublicDate)
            .AsNoTracking()
            .ToList();

        for (int i = 0; i < messages.Count; i++)
        {
            messages[i].Body = DataProtector.Decrypt(messages[i].Body);
        }

        return messages;
    }

    public async Task<IEnumerable<Message>?> GetAllMessageByChatSearch(int chatId, string bodySubStr)
    {
        if(bodySubStr == null) 
        {
            bodySubStr = "";
        }

        Chat? existingChat = _chatService.GetChatById(chatId);

        if (existingChat == null)
        {
            return [];
        }

        var messages = await _context.Messages
            .Include(m => m.MediaFiles)
            .Where(m => m.ChatId == existingChat.Id && m.Body.Contains(bodySubStr))
            .AsNoTracking()
            .ToListAsync();

        for (int i = 0; i < messages.Count; i++) 
        {
            messages[i].Body = DataProtector.Decrypt(messages[i].Body);
        }

        return messages;
    }

    public Message? GetMessageById(Chat chat, int messageId)
    {
        return _context.Messages
            .Include(m => m.MediaFiles)
            .SingleOrDefault(m => m.Id == messageId);
    }

    public Task UpdateChat(Chat chat)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteMessage(string userId, int messageId) 
    {
        // Получаем сообщение из базы вместе с зависимостями (медиафайлами)
        var message = await _context.Messages
            .Include(m => m.MediaFiles)
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
        {
            return false;
        }

        // Проверка прав: только автор может удалить сообщение
        if (message.UserId != userId)
        {
            return false;
        }

        // Удаляем связанные файлы вручную, если в базе не настроено каскадное удаление Cascade
        if (message.MediaFiles != null)
        {
            _context.MediaFiles.RemoveRange(message.MediaFiles);
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return true;
    }
    
}
