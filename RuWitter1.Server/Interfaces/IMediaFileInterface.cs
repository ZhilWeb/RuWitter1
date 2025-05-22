using RuWitter1.Server.Models;
namespace RuWitter1.Server.Interfaces;

public interface IMediaFileInterface
{
    Task<IEnumerable<MediaFile>> GetAll();
    Task<IFormFile?> Download(int id);
    Task<IFormFile?> Upload(IFormFile mediaFile);
    Task Delete(int id);
    Task Update(IFormFile mediaFile);
}
