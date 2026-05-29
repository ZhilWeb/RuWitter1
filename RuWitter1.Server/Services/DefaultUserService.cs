namespace RuWitter1.Server.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;
using System.Threading.Tasks;

public class DefaultUserService : IDefaultUserInterface
{
    private readonly UserManager<DefaultUser> _userManager;
    private readonly IMediaFileInterface _mediaFileService;

    public DefaultUserService(UserManager<DefaultUser> userManager, IMediaFileInterface mediaFileService)
    {
        _userManager = userManager;
        _mediaFileService = mediaFileService;
    }

    public async Task<DefaultUser?> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<IEnumerable<DefaultUser>> GetAllUsersAsync()
    {
        return _userManager.Users.ToList();
    }

    public async Task<IEnumerable<DefaultUserPersonalDataUpdate>> GetAllUsersAsyncBySearch(string userId, string phoneNumber, string nickname, int ageFrom, int ageTo, string city, string interests)
    {
        if (phoneNumber == null) phoneNumber = "";
        if (nickname == null) nickname = "";
        if (ageFrom == default) ageFrom = 1;
        if (ageTo == default) ageTo = 100;
        if (city == null) city = "";
        if (interests == null) interests = "";

        List<DefaultUser> users = _userManager.Users
            .Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(phoneNumber))
            .Where(u => u.Nickname.Contains(nickname))
            .Where(u => u.Age >= ageFrom && u.Age < ageTo)
            .Where(u => u.City.Contains(city))
            .Where(u => u.Interests.Contains(interests))
            .Where(u => u.Id != userId)
            .ToList();

        List<DefaultUserPersonalDataUpdate> usersPersonalData = new List<DefaultUserPersonalDataUpdate>();
        foreach (var user in users) 
        {
            string phoneNumberPersonalData = user.PhoneNumber == null ? "" : user.PhoneNumber;

            DefaultUserPersonalDataUpdate personalData = new DefaultUserPersonalDataUpdate
            {
                Avatar = user.Avatar,
                UserId = user.Id,
                PhoneNumber = phoneNumberPersonalData,
                Nickname = user.Nickname,
                Age = user.Age,
                BriefInformation = user.BriefInformation,
                City = user.City,
                Interests = user.Interests
            };
            usersPersonalData.Add(personalData);
        }
        return usersPersonalData;
    }

    public async Task<IdentityResult> UpdateUserAsync(DefaultUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<DefaultUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IdentityResult> UpdatePersonalDataAsync(DefaultUserPersonalDataUpdate updatedUserData)
    {
        var existingUser = await _userManager.FindByIdAsync(updatedUserData.UserId);

        if(existingUser == null) 
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        // existingUser.PhoneNumber = updatedUserData.PhoneNumber;
        existingUser.Nickname = updatedUserData.Nickname;
        existingUser.Age = updatedUserData.Age;
        existingUser.BriefInformation = updatedUserData.BriefInformation;
        existingUser.City = updatedUserData.City;
        existingUser.Interests = updatedUserData.Interests;

        return await _userManager.UpdateAsync(existingUser);
    }

    public async Task<IdentityResult> UpdateAvatarAsync(IFormFile mediaFile, string userId)
    {
        Guid? newFileName = await _mediaFileService.Upload(mediaFile);

        if (newFileName is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Image for avatar not uploaded." });
        }

        MediaFile? newAvatar = await _mediaFileService.GetByName(newFileName);

        if (newAvatar == null) 
        {
            return IdentityResult.Failed(new IdentityError { Description = "Image for avatar not found." });
        }

        var existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        existingUser.AvatarId = newAvatar.Id;
        return await _userManager.UpdateAsync(existingUser);
    }
}
