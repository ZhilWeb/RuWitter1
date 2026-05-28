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

        public PostController(IPostInterface postService, UserManager<DefaultUser> userManager, ICommunityInterface communityService)
        {
            _postService = postService;
            _userManager = userManager;
            _communityService = communityService;
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


        // POST api/<PostController>/community/2/post/4
        [HttpPost("/community/post/{postId}")]
        public async Task<IActionResult> LikeByCommunity(int postId) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            Post? post = _postService.GetPostById(postId);
            if(post == null || post.CommunityId == 0) 
            {
                return NotFound();
            }
            Community? community = _communityService.GetCommunityById(post.CommunityId);
            if(community == null || community.Id == 0) 
            {
                return NotFound();
            }
            

            int resultPostId = await _postService.SetLikeByCommunity(userId, community.Id, postId);
            return Ok(resultPostId);
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

    }
}
