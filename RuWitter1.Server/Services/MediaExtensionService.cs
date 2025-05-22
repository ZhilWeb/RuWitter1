using RuWitter1.Server.Models;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;

namespace RuWitter1.Server.Services;

public class MediaExtensionService : IMediaExtensionInterface
{
    private readonly PostContext _context;

    public MediaExtensionService(PostContext context)
    {
        _context = context;
    }

    public async Task<MediaExtension?> Create(MediaExtension mediaExtension)
    {
        await _context.MediaExtensions.AddAsync(mediaExtension);

        // var findMediaExtension = await _context.MediaExtensions.FindAsync(mediaExtension.Id);

        await _context.SaveChangesAsync();

        return mediaExtension;
    }

    public async Task<IEnumerable<MediaExtension>> GetAll()
    {
        return await _context.MediaExtensions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MediaExtension?> GetById(int id)
    {
        return await _context.MediaExtensions
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<MediaExtension?> GetByName(string name)
    {
        return await _context.MediaExtensions
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Name == name);
    }
}
