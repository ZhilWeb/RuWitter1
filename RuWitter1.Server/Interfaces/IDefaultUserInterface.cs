using Microsoft.AspNetCore.Identity;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces;

public interface IDefaultUserInterface
{
    Task<IEnumerable<DefaultUser>> GetAllUsersAsync();
    Task<DefaultUser?> GetUserByIdAsync(string id);
    Task<DefaultUser?> GetUserByUsernameAsync(string username);
    Task<IdentityResult> UpdateUserAsync(DefaultUser user);
    Task<IdentityResult> DeleteUserAsync(string userId);
    Task<IdentityResult> UpdatePersonalDataAsync(DefaultUserPersonalDataUpdate updatedUserData);
    Task<IdentityResult> UpdateAvatarAsync(IFormFile mediaFile, string userId);
}
