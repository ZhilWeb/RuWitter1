using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;
using System.Collections;

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
            return;
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


        // записываем записи как просмотренные
        List<CommunityPostWatches> communityPostWatches = new List<CommunityPostWatches>();
        foreach (var post in posts)
        {
            if (post.CommunityId != null) 
            {
                CommunityPostWatches communityPostWatch = new CommunityPostWatches
                {
                    DefaultUserId = userId,
                    CommunityId = (int)post.CommunityId,
                    PostId = post.Id,
                };
                communityPostWatches.Add(communityPostWatch);
            }
            
        }
        await _context.CommunityPostWatches.AddRangeAsync(communityPostWatches);
        await _context.SaveChangesAsync();

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

    public async Task<int> SetLike(string userId, int postId, int? commentId = null) 
    {
        PostsLikes like = new PostsLikes 
        {
            DefaultUserId = userId,
            PostId = postId,
            CommentId = commentId
        };

        await _context.PostsLikes.AddAsync(like);
        await _context.SaveChangesAsync();
        return postId;
    }

    public async Task<int> SetLikeByCommunity(string userId, int communityId, int postId, int? commentId = null)
    {
        CommunityPostsLikes like = new CommunityPostsLikes
        {
            DefaultUserId = userId,
            CommunityId = communityId,
            PostId = postId,
            CommentId = commentId
        };

        await _context.CommunityPostsLikes.AddAsync(like);
        await _context.SaveChangesAsync();
        return postId;
    }

    public async Task<bool> DeleteLike(string userId, int postId, int? commentId = null)
    {
        PostsLikes? postLike = await _context.PostsLikes
            .FirstOrDefaultAsync(w => w.DefaultUserId == userId && w.PostId == postId && w.CommentId == commentId);

        if (postLike == null)
        {
            return false;
        }

        _context.PostsLikes.Remove(postLike);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLikeByCommunity(string userId, int communityId, int postId, int? commentId = null)
    {
        CommunityPostsLikes? postLike = await _context.CommunityPostsLikes
            .FirstOrDefaultAsync(w => w.DefaultUserId == userId && w.CommunityId == communityId && w.PostId == postId && w.CommentId == commentId);

        if (postLike == null)
        {
            return false;
        }

        _context.CommunityPostsLikes.Remove(postLike);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePostWatches(string userId) 
    {
        List<CommunityPostWatches> communityPostWatches = await _context.CommunityPostWatches
            .Where(w => w.DefaultUserId == userId)
            .ToListAsync();

        if(communityPostWatches.Count == 0) 
        {
            return false;
        }

        _context.CommunityPostWatches.RemoveRange(communityPostWatches);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<IEnumerable<Post>?> GetPostsBySearch(string userId, string postSubString, 
        string communityNameSubString, List<int> communityCategoryIds, DateTime dateTimeFrom, DateTime dateTimeTo) 
    {
        // устанавливаем значения по умолчанию
        if (postSubString == null) 
        {
            postSubString = "";
        }

        if (communityNameSubString == null)
        {
            communityNameSubString = "";
        }

        if (communityCategoryIds.Count == 0) 
        {
            communityCategoryIds = await _context.CommunityCategories
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
                .ToListAsync();
        }

        if (dateTimeFrom == default(DateTime)) 
        {
            dateTimeFrom = new DateTime();
        }

        if (dateTimeTo == default(DateTime)) 
        {
            dateTimeTo = new DateTime();
        }
        Console.WriteLine(postSubString);
        Console.WriteLine(communityNameSubString);
        Console.WriteLine(dateTimeFrom);
        Console.WriteLine(dateTimeTo);

        // получаем лайки
        List<CommunityPostsLikes> communityPostsLikes = _context.CommunityPostsLikes
            .Where(l => l.DefaultUserId == userId && l.CommentId == null)
            .ToList();

        List<int> communityPostsLikesIds = communityPostsLikes
            .OrderBy(l => l.PostId)
            .Select(l => l.PostId)
            .ToList();
        List<string> communityPostsLikesTexts = _context.Posts
            .Where(p => communityPostsLikesIds.Contains(p.Id))
            .OrderBy(p => p.Id)
            .Select(p => p.Body)
            .ToList();

        // получаем записи по введенным параметрам
        List<Post> posts = await _context.Posts
            .Include(p => p.MediaFiles)
            .Where(p => p.Body.Contains(postSubString))
            .Where(p => p.Community != null && p.Community.Name.Contains(communityNameSubString))
            .Where(p => p.Community != null && p.Community.CommunityCategoryId != null && communityCategoryIds.Contains((int)p.Community.CommunityCategoryId))
            .Where(p => p.PublicDate > dateTimeFrom && p.PublicDate < dateTimeTo)
            .ToListAsync();

        List<int> postIds = posts
            .OrderBy(p => p.Id)
            .Select(p => p.Id)
            .ToList();
        List<string> postTexts = posts
            .OrderBy(p => p.Id)
            .Select(p => p.Body)
            .ToList();



        // получаем рекомендации
        List<int> recommendations = await _recommendationClient
            .GetPostReccomendsForSearch(communityPostsLikesIds, communityPostsLikesTexts, postIds, postTexts);

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

        return posts
            .OrderBy(p => sortRecommendations[p.Id])
            .ToList();
    }
}
