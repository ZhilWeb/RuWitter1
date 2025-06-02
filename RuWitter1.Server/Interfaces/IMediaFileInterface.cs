using Microsoft.AspNetCore.Identity;
using RuWitter1.Server.Models;
namespace RuWitter1.Server.Interfaces;

public interface IMediaFileInterface
{
    Task<IEnumerable<MediaFile>> GetAll();
    Task<MediaFile?> Download(int id);

    Task<MediaFile?> GetByName(Guid? name);
    Task<Guid?> Upload(IFormFile mediaFile);
    Task Delete(int id);
    Task Update(IFormFile mediaFile);
}
