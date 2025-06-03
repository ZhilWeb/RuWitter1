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

        public PostController(IPostInterface postService, UserManager<DefaultUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        // GET: api/<PostController>
        [HttpGet]
        public IEnumerable<Post>? Get()
        {
            return _postService.GetAllPosts();
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async Task<Post?> Get(int id)
        {
            return await _postService.GetPostById(id);
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
    }
}
