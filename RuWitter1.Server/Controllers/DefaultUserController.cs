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
    public class DefaultUserController : ControllerBase
    {
        private readonly IDefaultUserInterface _defaultUserService;

        public DefaultUserController(IDefaultUserInterface defaultUserService)
        {
            _defaultUserService = defaultUserService;
        }

        // GET: api/<DefaultUserController>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _defaultUserService.GetAllUsersAsync();
            return Ok(users);
        }

        /*
        // GET api/<DefaultUserController>/5
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _defaultUserService.GetUserByUsernameAsync(username);
            if (user == null) 
            {
                return NotFound();
            }

            if (User.Identity.Name != user.UserName && !User.IsInRole("Admin")) 
            {
                return Forbid();
            }

            return Ok(user);
        }
        */

        // GET api/<DefaultUserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _defaultUserService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (User.Identity.Name != user.UserName && !User.IsInRole("Admin"))
            {
                // возвращаем общедоступные персональные данные
                var personalData = new DefaultUserPersonalDataUpdate
                {
                    UserId = user.Id,
                    Nickname = user.Nickname,
                    Age = user.Age,
                    BriefInformation = user.BriefInformation,
                    City = user.City,
                    Interests = user.Interests,
                    AvatarId = user.AvatarId,
                };

                return Ok(personalData);
            }

            return Ok(user);
        }


        // GET api/<DefaultUserController>/user
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unauthorized");
            }

            return Ok(userId);
        }


        // PUT api/<DefaultUserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] DefaultUser updatedUser)
        {
            if (id != updatedUser.Id) 
            {
                return BadRequest();
            }

            var existingUser = await _defaultUserService.GetUserByIdAsync(id);
            if (User.Identity.Name != existingUser.UserName && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _defaultUserService.UpdateUserAsync(updatedUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // PUT api/<DefaultUserController>/personaldata/5
        [HttpPut("personaldata/{id}")]
        public async Task<IActionResult> UpdatePersonalData([FromBody] DefaultUserPersonalDataUpdate updatedUserData)
        {
            if(updatedUserData == null) 
            {
                return BadRequest();
            }
            var existingUser = await _defaultUserService.GetUserByIdAsync(updatedUserData.UserId);

            if (existingUser == null)
            {
                return BadRequest();
            }

            if (User.Identity.Name != existingUser.UserName && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _defaultUserService.UpdatePersonalDataAsync(updatedUserData);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // PUT api/<DefaultUserController>/avatar
        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateAvatar(IFormFile formFile, string userId)
        {

            var existingUser = await _defaultUserService.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                return BadRequest();
            }

            if (User.Identity.Name != existingUser.UserName && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var result = await _defaultUserService.UpdateAvatarAsync(formFile, userId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { fileName = formFile.FileName, contentType = formFile.ContentType });
        }

        // DELETE api/<DefaultUserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _defaultUserService.GetUserByIdAsync(id);
            if (User.Identity.Name != existingUser.UserName && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _defaultUserService.DeleteUserAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
