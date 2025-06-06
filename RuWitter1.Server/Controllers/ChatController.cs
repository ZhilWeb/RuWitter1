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
    public class ChatController : ControllerBase
    {
        private readonly IChatInterface _chatService;
        private readonly UserManager<DefaultUser> _userManager;

        public ChatController(IChatInterface chatService, UserManager<DefaultUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        // GET: api/<ChatController>
        [HttpGet]
        public async Task<IEnumerable<Chat>?> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unauthorized");
            }

            DefaultUser currentUser = await _userManager.FindByIdAsync(userId);


            return _chatService.GetAllChatsByUser(currentUser);
        }

        // GET api/<ChatController>/5
        [HttpGet("{chatId}")]
        public async Task<Chat?> Get(int chatId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unauthorized");
            }

            DefaultUser currentUser = await _userManager.FindByIdAsync(userId);


            return _chatService.GetChatById(chatId);
        }

        // POST api/<ChatController>
        [HttpPost]
        public async Task<IActionResult> Post(string acceptorUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unauthorized");
            }

            DefaultUser? currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null) 
            {
                return NotFound("Current user has not found.");
            }


            DefaultUser? existingAcceptor = await _userManager.FindByIdAsync(acceptorUserId);

            if(existingAcceptor == null) 
            {
                return NotFound("Input acceptor does not exist");
            }

            await _chatService.CreateChat(currentUser, existingAcceptor);

            return Ok(new { userId, acceptorUserId });
        }

        // PUT api/<ChatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
