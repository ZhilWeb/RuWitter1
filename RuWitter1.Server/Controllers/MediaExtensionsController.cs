using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Services;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuWitter1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaExtensionsController : ControllerBase
    {
        private readonly IMediaExtensionInterface _mediaExtensionService;
        public MediaExtensionsController(IMediaExtensionInterface mediaExtensionService)
        {
            _mediaExtensionService = mediaExtensionService;
        }

        // GET: api/<MediaExtensionsController>
        // исключить при сдаче
        [HttpGet, Authorize]
        public async Task<IEnumerable<MediaExtension>> Get()
        {
            var mediaExtensions = await _mediaExtensionService.GetAll();
            return mediaExtensions;
        }

        // GET api/<MediaExtensionsController>/5
        [HttpGet("{id}"), Authorize]
        public async Task<MediaExtension?> Get(int id)
        {
            var mediaExtension = await _mediaExtensionService.GetById(id);

            return mediaExtension;
        }

        // POST api/<MediaExtensionsController>
        [HttpPost, Authorize]
        public async Task<MediaExtension?> Post([FromBody] MediaExtension newMediaExtension)
        {
            await _mediaExtensionService.Create(newMediaExtension);

            return newMediaExtension;
        }

        // PUT api/<MediaExtensionsController>/5
        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MediaExtensionsController>/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
