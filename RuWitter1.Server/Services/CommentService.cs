using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Services;

public class CommentService : ICommentInterface
{
    private readonly PostContext _context;
    // private readonly IDefaultUserInterface _defaultUserService;
    private readonly IMediaFileInterface _mediaFileService;
    private readonly IPostInterface _postService;

    public CommentService(PostContext context, IMediaFileInterface mediaFileService, IPostInterface postService)
    {
        _context = context;
        // _defaultUserService = defaultUserService;
        _mediaFileService = mediaFileService;
        _postService = postService;
    }

    public async Task CreateComment(string existingUserId, int postId, string body, List<IFormFile> files)
    {
        if (body == null)
        {
            throw new Exception("Body has invalid.");
        }

        Post? existingPost = await _postService.GetPostById(postId);

        if (existingPost == null) 
        {
            throw new Exception("Post has not found.");
        }


        if (files.Count == 0 || files == null)
        {
            // создание комментария на пост без файлов
            Comment comment = new Comment
            {
                UserId = existingUserId,
                Body = body,
                MediaFiles = null,
                PostId = existingPost.Id,
                RepliedCommentId = null
            };

            await _context.Comments.AddAsync(comment);
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

        // создание комментария на пост с файлами
        Comment commentWithFiles = new Comment
        {
            UserId = existingUserId,
            Body = body,
            MediaFiles = postMediaFiles,
            PostId = existingPost.Id,
            RepliedCommentId = null,
        };

        await _context.Comments.AddAsync(commentWithFiles);
        await _context.SaveChangesAsync();
        Console.WriteLine("Ok");
        return;
    }

    public async Task CreateRepliedComment(string existingUserId, int postId, int hostCommentId, string body, List<IFormFile> files)
    {
        if (body == null)
        {
            throw new Exception("Body has invalid.");
        }

        Post? existingPost = await _postService.GetPostById(postId);

        if (existingPost == null)
        {
            throw new Exception("Post has not found.");
        }

        Comment? existingHostComment = await _context.Comments
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == hostCommentId && c.PostId == existingPost.Id);

        if (existingHostComment == null) 
        {
            throw new Exception("Comment has not found.");
        }


        if (files.Count == 0 || files == null)
        {
            // создание комментария на комментарий без файлов
            Comment comment = new Comment
            {
                UserId = existingUserId,
                Body = body,
                MediaFiles = null,
                PostId = existingPost.Id,
                RepliedCommentId = existingHostComment.Id,
            };

            await _context.Comments.AddAsync(comment);
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

        // создание комментария на комментарий без файлов
        Comment commentWithFiles = new Comment
        {
            UserId = existingUserId,
            Body = body,
            MediaFiles = postMediaFiles,
            PostId = existingPost.Id,
            RepliedCommentId = existingHostComment.Id,
        };

        await _context.Comments.AddAsync(commentWithFiles);
        await _context.SaveChangesAsync();
        Console.WriteLine("Ok");
        return;
    }

    public Task DeletePost(int commentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>?> GetAllOfPost(int postId)
    {
        Post? existingPost = await _postService.GetPostById(postId);

        if (existingPost == null)
        {
            throw new Exception("Post has not found.");
        }

        return await _context.Comments
            .Include(c => c.MediaFiles)
            .Where(c => c.PostId == existingPost.Id)
            .OrderBy(p => p.Id)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Post?> GetCommentById(int commentId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateComment(Comment comment)
    {
        throw new NotImplementedException();
    }
}
