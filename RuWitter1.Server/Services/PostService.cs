using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Services;

public class PostService : IPostInterface
{
    private readonly PostContext _context;
    // private readonly IDefaultUserInterface _defaultUserService;
    private readonly IMediaFileInterface _mediaFileService;

    public PostService(PostContext context, IMediaFileInterface mediaFileService)
    {
        _context = context;
        // _defaultUserService = defaultUserService;
        _mediaFileService = mediaFileService;
    }

    public async Task CreatePost(string existingUserId, string body, List<IFormFile> files)
    {
        // var existingUser = await _defaultUserService.GetUserByIdAsync(userId);

        if(body == null) 
        {
            throw new Exception("Body has invalid.");
        }
        
        
        if(files.Count == 0 || files == null) 
        {
            // создание поста без файлов
            Post post = new Post
            {
                UserId = existingUserId,
                Body = body,
                MediaFiles = null,
                Comments = null
            };

            await _context.Posts.AddAsync(post);
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

        // создание поста с файлами
        Post postWithFiles = new Post 
        {
            UserId = existingUserId,
            Body = body,
            MediaFiles = postMediaFiles
        };

        await _context.Posts.AddAsync(postWithFiles);
        await _context.SaveChangesAsync();
        Console.WriteLine("Ok");
        return;
    }

    public Task DeletePost(int postId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Post>? GetAllPosts(int lastPostId)
    {
        return _context.Posts
            .Include(p => p.Comments)
            .Include(p => p.MediaFiles)
            .OrderBy(p => p.Id)
            .Where(p => p.Id > lastPostId)
            .Take(10)
            .AsNoTracking()
            .ToList();
    }

    public int GetCountOfPosts()
    {
        return _context.Posts.Count();
    }

    public async Task<Post?> GetPostById(int postId)
    {
        return await _context.Posts
            .Include(p => p.MediaFiles)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == postId);
    }

    public Task UpdatePost(Post post)
    {
        throw new NotImplementedException();
    }
}
