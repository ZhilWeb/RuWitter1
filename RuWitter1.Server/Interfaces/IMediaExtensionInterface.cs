using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IMediaExtensionInterface
{
    Task<IEnumerable<MediaExtension>> GetAll();
    Task<MediaExtension?> GetById(int id);
    Task<MediaExtension?> GetByName(string name);
    Task<MediaExtension?> Create(MediaExtension mediaExtension);
}
