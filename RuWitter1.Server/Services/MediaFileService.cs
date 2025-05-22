using RuWitter1.Server.Models;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace RuWitter1.Server.Services;

public class MediaFileService : IMediaFileInterface
{
    private readonly PostContext _context;

    public MediaFileService(PostContext context)
    {
        _context = context;
    }

    public async Task<IFormFile?> Upload(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            Console.WriteLine("No file uploaded.");
            return formFile; // BadRequest("No file uploaded.")
        }
        IEnumerable<MediaExtension> mediaExtensionsEnum = await _context.MediaExtensions.AsNoTracking().ToListAsync();
        List<MediaExtension> mediaExtensions = mediaExtensionsEnum.ToList();


        var fileName = Path.GetFileName(formFile.FileName);
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) || !mediaExtensions.Exists(m => m.Name == fileExtension))
        {
            Console.WriteLine("The extension is invalid");
            return formFile; // The extension is invalid
        }

        var newFileName = Guid.NewGuid(); // новое имя без расширения
        try
        {
            MediaExtension dbfileExtension = await _context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension); // не должен быть null, так как уже проверен
            var mediaFile = new MediaFile
            {
                Name = newFileName,
                ExtensionId = dbfileExtension.Id,
                UploadDate = DateTime.UtcNow,
            };
            // mediaFile.Extension = await _context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension);
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            mediaFile.Data = memoryStream.ToArray();

            await _context.MediaFiles.AddAsync(mediaFile);
            await _context.SaveChangesAsync();
            Console.WriteLine("Ok");
            return formFile;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error uploading file");
            return formFile;
        }

    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MediaFile>> GetAll()
    {
        return await _context.MediaFiles
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<IFormFile?> Download(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(IFormFile mediaFile)
    {
        throw new NotImplementedException();
    }
}
