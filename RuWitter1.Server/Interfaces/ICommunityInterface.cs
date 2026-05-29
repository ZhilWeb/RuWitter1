using Microsoft.AspNetCore.Mvc;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Interfaces
{
    public interface ICommunityInterface
    {
        IEnumerable<Community>? GetAllCommunities();
        IEnumerable<Community>? GetAllCommunitiesByUser(string existingUserId);
        Community? GetCommunityById(int? communityId);
        Task<string> CreateCommunity(string existingUserId, string name, IFormFile avatar, string briefInformation, int categoryId);
        Task UpdateCommunity(Community community);

        Task<IEnumerable<Community>> GetCommunityBySearch(string name,
            List<int> communityCategoryIds, string briefInformationSubstring, string managerName);
    }
}
