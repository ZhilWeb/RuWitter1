using System.Security.Claims;
using System.Security.Cryptography;
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
    public class MessageController : ControllerBase
    {
        private readonly IMessageInterface _messageService;
        private readonly IChatInterface _chatService;
        private readonly UserManager<DefaultUser> _userManager;

        public MessageController(IMessageInterface messageService, UserManager<DefaultUser> userManager, IChatInterface chatService)
        {
            _messageService = messageService;
            _userManager = userManager;
            _chatService = chatService;
        }

        // GET: api/<MessageController>/1
        [HttpGet("{chatId}")]
        public async Task<IEnumerable<Message>?> Get(int chatId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unauthorized");
            }

            DefaultUser? currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null) 
            {
                throw new Exception("Current user has not found.");
            }

            IEnumerable<Message>? messages = await _messageService.GetAllMessageByChat(currentUser, chatId);

            if (messages == null) 
            {
                throw new Exception("Message has not found.");
            }
            /*
            foreach(var message in messages) 
            {
                // поменять при docker
                byte[] decryptedBody = ProtectedData.Unprotect(
                    Convert.FromBase64String(message.Body),
                    null,
                    DataProtectionScope.CurrentUser
                    );

                message.Body = System.Text.Encoding.UTF8.GetString(decryptedBody);
            }
            */

            return messages;
        }

        // GET api/<MessageController>/1/5
        [HttpGet("{chatId}/{messageId}")]
        public string Get(int chatId, int messageId)
        {
            return "value";
        }

        // POST api/<MessageController>/1
        [HttpPost("{chatId}")]
        public async Task<IActionResult> Post(string body, int chatId, List<IFormFile> formFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }


            DefaultUser? currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {
                return NotFound("Current user has not found.");
            }

            if (body == null)
            {
                throw new Exception("Body has invalid.");
            }



            var chatUsersId = _chatService.GetUsersIdByChat(chatId);

            if (!chatUsersId.Contains(currentUser.Id)) 
            {
                return Forbid("User has not permissions for this chat.");
            }

            /*
            // поменять при docker
            byte[] encryptedBody = ProtectedData.Protect(
                System.Text.Encoding.UTF8.GetBytes(body),
                null,
                DataProtectionScope.CurrentUser
                );

            await _messageService.CreateMessage(currentUser, chatId, Convert.ToBase64String(encryptedBody), formFiles);
            */
            await _messageService.CreateMessage(currentUser, chatId, body, formFiles);
            return Ok(chatId);
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
