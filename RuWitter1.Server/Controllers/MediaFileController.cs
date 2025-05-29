using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MediaFileController>/5
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mediaFile = await _mediaFileService.Download(id);

                if (mediaFile == null)
                {
                    return NotFound();
                }

                return File(mediaFile.Data, mediaFile.ContentType, String.Concat(Convert.ToString(mediaFile.Name), mediaFile.Extension.Name));
            }
            catch (NullReferenceException ex) 
            {
                return NotFound();
            }
        }

        // POST api/<MediaFileController>
        [HttpPost, Authorize]
        public async Task<IActionResult> Post(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            IFormFile? oldFormFile = await _mediaFileService.Upload(formFile);

            return Ok(new { fileName = formFile.FileName, contentType = formFile.ContentType });
        }

        // PUT api/<MediaFileController>/5
        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MediaFileController>/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
