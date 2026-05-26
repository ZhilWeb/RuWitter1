using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunityInterface _communityService;
    private readonly PostContext _context;
    
    public CommunitiesController(ICommunityInterface communityService, PostContext context)
    {
        _communityService = communityService;
        _context = context;
    }

    // GET: api/Community
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Community>>> GetCommunity()
    {
        return await _context.Communities.ToListAsync();
    }

    // GET: api/Community/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Community>> GetCommunity(int id)
    {
        var community = await _context.Communities.FindAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        return community;
    }

    // PUT: api/Community/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCommunity(int? id, Community community)
    {
        if (id != community.Id)
        {
            return BadRequest();
        }

        _context.Entry(community).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommunityExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Community
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult> PostCommunity([FromForm] string name, IFormFile avatar, [FromForm] string briefInformation, [FromForm] int categoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        string addResult = await _communityService.CreateCommunity(userId, name, avatar, briefInformation, categoryId);
        return Ok(addResult);
    }

    // DELETE: api/Community/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommunity(int? id)
    {
        var community = await _context.Communities.FindAsync(id);
        if (community == null)
        {
            return NotFound();
        }

        _context.Communities.Remove(community);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommunityExists(int? id)
    {
        return _context.Communities.Any(e => e.Id == id);
    }
}
