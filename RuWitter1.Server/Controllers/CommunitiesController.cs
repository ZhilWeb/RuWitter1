using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;
using RuWitter1.Server.Services;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunityInterface _communityService;
    private readonly PostContext _context;
    private readonly IMediaFileInterface _mediaFileService;
    
    public CommunitiesController(ICommunityInterface communityService, PostContext context, IMediaFileInterface mediaFileService)
    {
        _communityService = communityService;
        _context = context;
        _mediaFileService = mediaFileService;
    }

    // GET: api/Community
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Community>>> GetCommunity()
    {
        return await _context.Communities.ToListAsync();
    }

    [HttpGet("categories")]
    public async Task<IEnumerable<CommunityCategory>> GetCommunitiesCategories() 
    {
        return await _communityService.GetCommunitiesCategories();
    }

    // GET: api/Community/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Community>> GetCommunity(int id)
    {
        Console.WriteLine(id);
        var community = _communityService.GetCommunityById(id);

        if (community == null)
        {
            return NotFound();
        }

        return community;
    }

    [HttpGet("isusercomm/{id}")]
    public async Task<bool> IsCurrentUserCommunityManager(int id) 
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }

        Community? community = await _context.Communities
            .FirstOrDefaultAsync(c => c.DefaultUserId == userId && c.Id == id);

        if(community == null) 
        {
            return false;
        }

        return true;
    }


    [HttpGet("currentusercomm")]
    public async Task<IEnumerable<Community>> GetCurrentUserCommunitites() 
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return [];
        }

        List<Community> communities = await _context.Communities
            .Where(c => c.DefaultUserId == userId)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return communities;
    }

    [HttpPost("updatebrief/{id}")]
    public async Task<IActionResult> UpdateCommunity(int id, [FromForm] string name, 
        [FromForm] List<IFormFile> avatars, [FromForm] int categoryId, [FromForm] string briefInformation) 
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        Community? community = await _context.Communities
            .FirstOrDefaultAsync(c => c.DefaultUserId == userId && c.Id == id);

        if (community == null)
        {
            return NotFound();
        }

        var categories = await _communityService.GetCommunitiesCategories();

        List<int> categoriesIds = categories.Select(c => c.Id).ToList();

        if (name == null || briefInformation == null || !categoriesIds.Contains(categoryId))
        {
            return BadRequest();
        }

        community.Name = name;
        community.CommunityCategoryId = categoryId;
        community.BriefInformation = briefInformation;
        if (avatars.Any()) 
        {
            if (community.AvatarId != null)
            {
                bool result = await _mediaFileService.Delete((int)community.AvatarId);
            }

            IFormFile avatar = avatars[0];

            if (avatar != null)
            {
                MediaFile? mediaFile = await _mediaFileService.InitMediaFile(avatar);
                community.Avatar = mediaFile;
            }
        }
        

        _context.Entry(community).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(id);
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
    public async Task<ActionResult> PostCommunity([FromForm] string name, [FromForm] List<IFormFile> avatars, [FromForm] string briefInformation, [FromForm] int categoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        string addResult = await _communityService.CreateCommunity(userId, name, avatars, briefInformation, categoryId);
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

    // GET: api/Community/search
    [HttpPost("search")]
    public async Task<IEnumerable<Community>> GetCommunityBySearch([FromForm] List<int> communityCategoryIds, [FromForm] string name = "",
            [FromForm] string briefInformationSubstring = "", [FromForm] string managerName = "")
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine(userId);
        if (string.IsNullOrEmpty(userId))
        {
            return [];
        }

        return await _communityService.GetCommunityBySearch(name, communityCategoryIds, briefInformationSubstring, managerName);
    }

    private bool CommunityExists(int? id)
    {
        return _context.Communities.Any(e => e.Id == id);
    }
}
