using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Interfaces;
using RuWitter1.Server.Models;

namespace RuWitter1.Server.Services
{
    public class CommunityService : ICommunityInterface
    {
        private readonly PostContext _context;
        private readonly IMediaFileInterface _mediaFileService;
        public CommunityService(PostContext context, IMediaFileInterface mediaFileService)
        {
            _context = context;
            // _defaultUserService = defaultUserService;
            _mediaFileService = mediaFileService;
        }

        public async Task<string> CreateCommunity(string existingUserId, string name, IFormFile avatar, string briefInformation, int categoryId)
        {
            // на пользователя проверяем в контроллере

            // добавление аватара
            MediaFile? avatarFile = await _mediaFileService.InitMediaFile(avatar);


            Community community = new Community
            {
                DefaultUserId = existingUserId,
                Name = name,
                AvatarId = avatarFile?.Id,
                BriefInformation = briefInformation,
                CommunityCategoryId = categoryId
            };

            await _context.Communities.AddAsync(community);
            await _context.SaveChangesAsync();
            Console.WriteLine("Ok");
            return $"Сообщество {name} создано.";
        }

        public IEnumerable<Community>? GetAllCommunities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Community>? GetAllCommunitiesByUser(string existingUserId)
        {
            throw new NotImplementedException();
        }

        public Community? GetCommunityById(int? communityId)
        {
            return _context.Communities
                .Include(c => c.Avatar)
                .Include(c => c.Posts)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == communityId);
        }

        public Task UpdateCommunity(Community community)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Community>> GetCommunityBySearch(string name, 
            List<int> communityCategoryIds, string briefInformationSubstring, string managerName) 
        {
            // устанавливаем значения по умолчанию
            if (name == null)
            {
                name = "";
            }

            if (briefInformationSubstring == null)
            {
                briefInformationSubstring = "";
            }

            if (managerName == null)
            {
                managerName = "";
            }

            if (communityCategoryIds.Count == 0)
            {
                communityCategoryIds = await _context.CommunityCategories
                    .OrderBy(c => c.Id)
                    .Select(c => c.Id)
                    .ToListAsync();
            }

            List<Community> communitites = await _context.Communities
                .Include(c => c.Avatar)
                .Where(c => c.Name.Contains(name))
                .Where(c => c.BriefInformation.Contains(briefInformationSubstring))
                .Where(c => c.DefaultUser != null && c.DefaultUser.Nickname.Contains(managerName))
                .Where(c => c.CommunityCategoryId != null && communityCategoryIds.Contains((int)c.CommunityCategoryId))
                .ToListAsync();

            return communitites;
        }
    }
}
