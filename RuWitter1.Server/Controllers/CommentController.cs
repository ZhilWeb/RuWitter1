using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;
using RuWitter1.Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuWitter1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentInterface _commentService;
        private readonly UserManager<DefaultUser> _userManager;

        public CommentController(ICommentInterface commentService, UserManager<DefaultUser> userManager) 
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // GET: api/<CommentController>
        [HttpGet("{postId}")]
        public async Task<IEnumerable<Comment>?> Get(int postId)
        {
            return await _commentService.GetAllOfPost(postId);
        }

        // GET api/<CommentController>/1/5
        [HttpGet("{postId}/{commentId}")]
        public string Get(int postId, int commentId)
        {
            return "value";
        }

        // POST api/<CommentController>/1
        [HttpPost("{postId}")]
        public async Task<IActionResult> Post(string body, int postId, List<IFormFile> formFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _commentService.CreateComment(userId, postId, body, formFiles);
            return Ok(userId);
        }

        // POST api/<CommentController>/1/5
        [HttpPost("{postId}/{hostCommentId}")]
        public async Task<IActionResult> RepliedPost(string body, int postId, int hostCommentId, List<IFormFile> formFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _commentService.CreateRepliedComment(userId, postId, hostCommentId, body, formFiles);
            return Ok(userId);
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
