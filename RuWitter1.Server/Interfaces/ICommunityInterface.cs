using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces
{
    public interface ICommunityInterface
    {
        IEnumerable<Community>? GetAllCommunities();
        IEnumerable<Community>? GetAllCommunitiesByUser(string existingUserId);
        Task<Community?> GetCommunityById(int communityId);
        Task<string> CreateCommunity(string existingUserId, string name, IFormFile avatar, string briefInformation, int categoryId);
        Task UpdateCommunity(Community community);
    }
}
