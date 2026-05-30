using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuWitter1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostInterface _postService;
        private readonly UserManager<DefaultUser> _userManager;
        private readonly ICommunityInterface _communityService;
        private readonly ICommentInterface _commentService;

        public PostController(IPostInterface postService, UserManager<DefaultUser> userManager, 
            ICommunityInterface communityService, ICommentInterface commentService)
        {
            _postService = postService;
            _userManager = userManager;
            _communityService = communityService;
            _commentService = commentService;
        }

        // GET: api/<PostController>/count
        [HttpGet("count")]
        public int GetCountOfPosts()
        {
            return _postService.GetCountOfPosts();
        }

        // GET: api/<PostController>
        [HttpGet("{lastPostId}")]
        public IEnumerable<Post>? Get(int lastPostId)
        {
            return _postService.GetAllPosts(lastPostId);
        }

        // GET api/<PostController>/post/5
        [HttpGet("post/{id}")]
        public Post? GetById(int id)
        {
            return _postService.GetPostById(id);
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task<IActionResult> Post(string body, List<IFormFile> formFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _postService.CreatePost(userId, body, formFiles);
            return Ok(userId);
        }

        // POST api/<PostController>/community/2
        [HttpPost("/community/{communityId}")]
        public async Task<IActionResult> PostByCommunity([FromForm] string body, int communityId, List<IFormFile> formFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var community = _communityService.GetCommunityById(communityId);

            if (string.IsNullOrEmpty(userId) || community == null || community.Id == 0 || userId != community.DefaultUserId)
            {
                return Unauthorized();
            }

            await _postService.CreatePostByCommunity(userId, communityId, body, formFiles);
            return Ok(communityId);
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        

        // GET: api/<PostController>
        [HttpGet]
        public async Task<IEnumerable<Post>?> GetPostsFeed() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return [];
            }

            return await _postService.GetPostsByNewsFeed(userId);
        }

        // Delete: api/<PostController>/postwatches
        [HttpDelete("postwatches")]
        public async Task<IActionResult> DeletePostWatches() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            bool deletePostWatchesResult = await _postService.DeletePostWatches(userId);
            if (!deletePostWatchesResult) 
            {
                return NotFound("Данные о просмотрах записей сообществ не найдены.");
            }

            return Ok("Данные о просмотрах записей сообществ удалены.");
        }

        // POST: api/<PostController>/search
        [HttpPost("search")]
        public async Task<IEnumerable<Post>?> GetPostBySearch([FromForm] List<int> communityCategoryIds, 
            [FromForm] DateTime dateTimeFrom, [FromForm] DateTime dateTimeTo, 
            [FromForm] string postSubString = "", [FromForm] string communityNameSubString = "") 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return [];
            }

            return await _postService.GetPostsBySearch(userId, postSubString, communityNameSubString, communityCategoryIds, dateTimeFrom, dateTimeTo);
        }




        // POST api/<PostController>/community/like/4
        [HttpPost("community/like/{postId}")]
        public async Task<IActionResult> SetLikeByCommunity(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community == null || community.Id == 0)
            {
                return NotFound();
            }


            int resultPostId = await _postService.SetLikeByCommunity(userId, community.Id, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/like/4
        [HttpPost("/like/{postId}")]
        public async Task<IActionResult> SetLike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }

            // не принадлежит обществу
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community != null || community.Id != 0)
            {
                return NotFound();
            }


            int resultPostId = await _postService.SetLike(userId, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/community/like/4/5
        [HttpPost("/community/like/{postId}/{commentId}")]
        public async Task<IActionResult> SetCommentLikeByCommunity(int postId, int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community == null || community.Id == 0)
            {
                return NotFound();
            }

            Comment? comment = await _commentService.GetCommentById(postId, commentId);

            if (comment == null || comment.Id != commentId || comment.PostId != postId)
            {
                return NotFound();
            }

            int resultPostId = await _postService.SetLikeByCommunity(userId, community.Id, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/like/4
        [HttpPost("/like/{postId}/{commentId}")]
        public async Task<IActionResult> SetCommentLike(int postId, int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }

            // не принадлежит обществу
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community != null || community.Id != 0)
            {
                return NotFound();
            }

            Comment? comment = await _commentService.GetCommentById(postId, commentId);

            if (comment == null || comment.Id != commentId || comment.PostId != postId)
            {
                return NotFound();
            }


            int resultPostId = await _postService.SetLike(userId, postId, null);
            return Ok(resultPostId);
        }



        // POST api/<PostController>/delete/community/like/4
        [HttpPost("delete/community/like/{postId}")]
        public async Task<IActionResult> DeleteLikeByCommunity(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community == null || community.Id == 0)
            {
                return NotFound();
            }


            bool resultPostId = await _postService.DeleteLikeByCommunity(userId, community.Id, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/delete/like/4
        [HttpPost("delete/like/{postId}")]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }

            // не принадлежит обществу
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community != null || community.Id != 0)
            {
                return NotFound();
            }


            bool resultPostId = await _postService.DeleteLike(userId, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/delete/community/like/4/5
        [HttpPost("delete/community/like/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteCommentLikeByCommunity(int postId, int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community == null || community.Id == 0)
            {
                return NotFound();
            }

            Comment? comment = await _commentService.GetCommentById(postId, commentId);

            if (comment == null || comment.Id != commentId || comment.PostId != postId)
            {
                return NotFound();
            }

            bool resultPostId = await _postService.DeleteLikeByCommunity(userId, community.Id, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/delete/like/4
        [HttpPost("delete/like/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteCommentLike(int postId, int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return NotFound();
            }

            // не принадлежит обществу
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community != null || community.Id != 0)
            {
                return NotFound();
            }

            Comment? comment = await _commentService.GetCommentById(postId, commentId);

            if (comment == null || comment.Id != commentId || comment.PostId != postId)
            {
                return NotFound();
            }


            bool resultPostId = await _postService.DeleteLike(userId, postId, null);
            return Ok(resultPostId);
        }

        // POST api/<PostController>/issetlike
        [HttpPost("issetlike")]
        public async Task<bool> IsSetLike([FromForm] int postId, [FromForm] int? commentId) 
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            Post? post = _postService.GetPostById(postId);
            if (post == null || post.Id == 0)
            {
                return false;
            }

            bool hasCommunity = true;
            bool hasComment = true;
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if (community == null || community.Id == 0) 
            {
                hasCommunity = false;
            }

            Comment? comment = await _commentService.GetCommentById(postId, commentId);
            if (comment == null || comment.Id != commentId || comment.PostId != postId)
            {
                hasComment = false;
            }

            bool hasLike = false;
            if (!hasCommunity && !hasComment) 
            {
                hasLike = await _postService.IsSetPostLikeByUser(userId, postId, null);
                
            }
            if(hasCommunity && !hasComment) 
            {
                hasLike = await _postService.IsSetCommunityPostLikeByUser(userId, community.Id, postId, null);
            }
            if (!hasCommunity && hasComment)
            {
                hasLike = await _postService.IsSetPostLikeByUser(userId, postId, commentId);

            }
            if (hasCommunity && hasComment)
            {
                hasLike = await _postService.IsSetCommunityPostLikeByUser(userId, community.Id, postId, commentId);
            }
            return hasLike;
        }
    }
}
