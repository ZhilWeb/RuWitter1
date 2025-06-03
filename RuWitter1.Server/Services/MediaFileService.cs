using RuWitter1.Server.Models;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace RuWitter1.Server.Services;

public class MediaFileService : IMediaFileInterface
{
    private readonly PostContext _context;
    private readonly IMediaExtensionInterface _mediaExtensionService;

    public MediaFileService(PostContext context, IMediaExtensionInterface mediaExtensionService)
    {
        _context = context;
        _mediaExtensionService = mediaExtensionService;
    }

    public async Task<Guid?> Upload(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            Console.WriteLine("No file uploaded.");
            return null; // BadRequest("No file uploaded.")
        }
        IEnumerable<MediaExtension> mediaExtensionsEnum = await _mediaExtensionService.GetAll();
        List<MediaExtension> mediaExtensions = mediaExtensionsEnum.ToList();


        var fileName = Path.GetFileName(formFile.FileName);
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) || !mediaExtensions.Exists(m => m.Name == fileExtension))
        {
            Console.WriteLine("The extension is invalid");
            return null; // The extension is invalid
        }

        var newFileName = Guid.NewGuid(); // новое имя без расширения
        try
        {
            MediaExtension dbfileExtension = await _mediaExtensionService.GetByName(fileExtension); // не должен быть null, так как уже проверен
            var mediaFile = new MediaFile
            {
                Name = newFileName,
                ContentType = formFile.ContentType,
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
            return mediaFile.Name;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error uploading file");
            return null;
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

    public async Task<MediaFile?> Download(int id)
    {
        MediaFile? mediaFile = await _context.MediaFiles
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == id);

        MediaExtension? fileExtension = await _mediaExtensionService.GetById(mediaFile.ExtensionId); // установка MediaExtension

        if (fileExtension is not null) 
        {
            mediaFile.Extension = fileExtension;
        }

        return mediaFile;
    }

    public Task Update(IFormFile mediaFile)
    {
        throw new NotImplementedException();
    }

    public async Task<MediaFile?> GetByName(Guid? name)
    {
        return await _context.MediaFiles
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Name == name);
    }

    public async Task<MediaFile?> InitMediaFile(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            Console.WriteLine("No file uploaded.");
            return null; // BadRequest("No file uploaded.")
        }
        IEnumerable<MediaExtension> mediaExtensionsEnum = await _mediaExtensionService.GetAll();
        List<MediaExtension> mediaExtensions = mediaExtensionsEnum.ToList();


        var fileName = Path.GetFileName(formFile.FileName);
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) || !mediaExtensions.Exists(m => m.Name == fileExtension))
        {
            Console.WriteLine("The extension is invalid");
            return null; // The extension is invalid
        }

        var newFileName = Guid.NewGuid(); // новое имя без расширения
        try
        {
            MediaExtension dbfileExtension = await _mediaExtensionService.GetByName(fileExtension); // не должен быть null, так как уже проверен
            var mediaFile = new MediaFile
            {
                Name = newFileName,
                ContentType = formFile.ContentType,
                ExtensionId = dbfileExtension.Id,
                UploadDate = DateTime.UtcNow,
            };
            // mediaFile.Extension = await _context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension);
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            mediaFile.Data = memoryStream.ToArray();

            return mediaFile;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error init file");
            return null;
        }
    }
}
