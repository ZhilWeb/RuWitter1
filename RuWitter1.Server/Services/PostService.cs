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

    private readonly RecommendationClient _recommendationClient;

    public PostService(PostContext context, IMediaFileInterface mediaFileService, RecommendationClient recommendationClient)
    {
        _context = context;
        // _defaultUserService = defaultUserService;
        _mediaFileService = mediaFileService;
        _recommendationClient = recommendationClient;
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

    public async Task CreatePostByCommunity(string existingUserId, int communityId, string body, List<IFormFile> files)
    {
        // var existingUser = await _defaultUserService.GetUserByIdAsync(userId);

        if (body == null)
        {
            throw new Exception("Body has invalid.");
        }


        if (files.Count == 0 || files == null)
        {
            // создание поста без файлов
            Post post = new Post
            {
                UserId = existingUserId,
                CommunityId = communityId,
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
            CommunityId = communityId,
            Body = body,
            MediaFiles = postMediaFiles,
            Comments = null
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
        IEnumerable<Comment>? comments = _context.Comments
            .Where(c => c.PostId == lastPostId)
            .AsNoTracking()
            .ToList();

        if(comments == null) 
        {
            return _context.Posts
            .Include(p => p.MediaFiles)
            .OrderBy(p => p.Id)
            .Where(p => p.Id > lastPostId)
            .Take(10)
            .AsNoTracking()
            .ToList();
        }


        return _context.Posts
            .Include(p => p.MediaFiles)
            .Include(p => p.Comments!)
            .ThenInclude(c => c.MediaFiles)
            .OrderBy(p => p.Id)
            .Where(p => p.Id > lastPostId)
            .Take(10)
            .AsNoTracking()
            .ToList();
    }

    public async Task<IEnumerable<Post>?> GetPostsByNewsFeed(string userId) 
    {
        // получаем лайки и просмотры
        List<int> communityPostsLikesIds = _context.CommunityPostsLikes
            .Where(l => l.DefaultUserId == userId && l.CommentId == null)
            .Select(l => l.PostId)
            .ToList();
        List<int> communityPostWatchesIds = _context.CommunityPostWatches
            .Where(l => l.DefaultUserId == userId)
            .Select(l => l.PostId)
            .ToList();

        // получаем тексты понравившихся записей
        List<string> postsTexts = _context.Posts
            .Where(p => communityPostsLikesIds.Contains(p.Id))
            .Select(p => p.Body)
            .ToList();

        // получаем рекомендации
        List<int> recommendations = await _recommendationClient.GetPostReccomends(communityPostsLikesIds, communityPostWatchesIds, postsTexts);

        Dictionary<int, int> sortRecommendations = recommendations
        .Select((id, index) => new
        {
            id,
            index
        })
        .ToDictionary(
            x => x.id,
            x => x.index
        );

        // получаем записи по id
        List<Post> posts = await _context.Posts
            .Include(p => p.MediaFiles)
            .Where(p => recommendations.Contains(p.Id))
            .ToListAsync();
        return posts
            .OrderBy(p => sortRecommendations[p.Id])
            .ToList();
    }


    public int GetCountOfPosts()
    {
        return _context.Posts.Count();
    }

    public Post? GetPostById(int postId)
    {
        IEnumerable<Comment>? comments = _context.Comments
            .Where(c => c.PostId == postId)
            .AsNoTracking()
            .ToList();

        if (comments == null)
        {
            return _context.Posts
            .Include(p => p.MediaFiles)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == postId);
        }


        return _context.Posts
            .Include(p => p.MediaFiles)
            .Include(p => p.Comments!)
            .ThenInclude(c => c.MediaFiles)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == postId);
    }

    public Task UpdatePost(Post post)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SetLike(string userId, int postId) 
    {
        PostsLikes like = new PostsLikes 
        {
            DefaultUserId = userId,
            PostId = postId
        };

        await _context.PostsLikes.AddAsync(like);
        await _context.SaveChangesAsync();
        return postId;
    }

    public async Task<int> SetLikeByCommunity(string userId, int communityId, int postId)
    {
        CommunityPostsLikes like = new CommunityPostsLikes
        {
            DefaultUserId = userId,
            CommunityId = communityId,
            PostId = postId
        };

        await _context.CommunityPostsLikes.AddAsync(like);
        await _context.SaveChangesAsync();
        return postId;
    }
}
