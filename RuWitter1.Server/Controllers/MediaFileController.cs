using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

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
        public async Task<MediaFile> Get(int id)
        {
            try
            {
                var mediaFile = await _mediaFileService.Download(id);

                if (mediaFile == null)
                {
                    throw new Exception("File has not found.");
                }

                return mediaFile;
            }
            catch (NullReferenceException ex) 
            {
                throw new Exception("File has not found.");
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
            Guid? newFileName = await _mediaFileService.Upload(formFile);

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
