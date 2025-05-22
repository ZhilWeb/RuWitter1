using System.IO;
using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuWitter1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaFileController : ControllerBase
    {
        private readonly IMediaFileInterface _mediaFileService;

        public MediaFileController(IMediaFileInterface mediaFileService)
        {
            _mediaFileService = mediaFileService;
        }

        // GET: api/<MediaFileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MediaFileController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MediaFileController>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            IFormFile? oldFormFile = await _mediaFileService.Upload(formFile);

            return Ok(new { fileName = formFile.FileName });
        }

        // PUT api/<MediaFileController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MediaFileController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
