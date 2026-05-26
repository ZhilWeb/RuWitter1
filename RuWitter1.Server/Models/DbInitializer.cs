using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Services;
using System;
using System.IO;


namespace RuWitter1.Server.Models
{
    public static class DbInitializer
    {
        // категории сообществ (заданы хардкодом, так как должны быть отсортированы в связи с nameBriefCommunities)
        private static List<string> communitiesCategories = new List<string>() 
        {
            "Спорт",
            "Политика",
            "Общество",
            "Культура",
            "Наука_техника",
            "Происшествия",
            "Экономика",
        };

        // имена и описания сообществ
        private static List<Dictionary<string, string>> nameBriefCommunities = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() { { "name", "Все о спорте" }, { "brief", "Одними из первых обсуждаем спортивные новости." } },
                new Dictionary<string, string>() { { "name", "Все о спорте и даже больше" }, { "brief", "Здесь все про спорт." } },
                new Dictionary<string, string>() { { "name", "Про спорт" }, { "brief", "Обсуждаем спорт." } },
                new Dictionary<string, string>() { { "name", "Все о политике" }, { "brief", "Одними из первых обсуждаем политические новости." } },
                new Dictionary<string, string>() { { "name", "Все о политике и даже больше" }, { "brief", "Здесь все про политику." } },
                new Dictionary<string, string>() { { "name", "Про политику" }, { "brief", "Обсуждаем политику." } },
                new Dictionary<string, string>() { { "name", "Все об обществе" }, { "brief", "Одними из первых обсуждаем общественные новости." } },
                new Dictionary<string, string>() { { "name", "Все об обществе и даже больше" }, { "brief", "Здесь все про общество." } },
                new Dictionary<string, string>() { { "name", "Про общество" }, { "brief", "Обсуждаем общество." } },
                new Dictionary<string, string>() { { "name", "Все о культуре" }, { "brief", "Одними из первых обсуждаем новости культуры." } },
                new Dictionary<string, string>() { { "name", "Все о культуре и даже больше" }, { "brief", "Здесь все про культуру." } },
                new Dictionary<string, string>() { { "name", "Про культуру" }, { "brief", "Обсуждаем культуру." } },
                new Dictionary<string, string>() { { "name", "Все о науке и технике" }, { "brief", "Одними из первых обсуждаем новости науки и техники." } },
                new Dictionary<string, string>() { { "name", "Все о науке и технике и даже больше" }, { "brief", "Здесь все про науку и технику." } },
                new Dictionary<string, string>() { { "name", "Про науку и технику" }, { "brief", "Обсуждаем науку и технику." } },
                new Dictionary<string, string>() { { "name", "Все о происшествиях" }, { "brief", "Одними из первых обсуждаем новости про происшествия." } },
                new Dictionary<string, string>() { { "name", "Все о происшествиях и даже больше" }, { "brief", "Здесь все про происшествия." } },
                new Dictionary<string, string>() { { "name", "Про происшествия" }, { "brief", "Обсуждаем происшествия." } },
                new Dictionary<string, string>() { { "name", "Все об экономике" }, { "brief", "Одними из первых обсуждаем новости про экономику." } },
                new Dictionary<string, string>() { { "name", "Все об экономике и даже больше" }, { "brief", "Здесь все про экономику." } },
                new Dictionary<string, string>() { { "name", "Про экономику" }, { "brief", "Обсуждаем экономику." } },
            };
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PostContext>();

            // 1. Извлекаем обязательные зависимости из базы
            var userIds = await context.Users.Select(u => u.Id).ToListAsync();
            var categories = await context.CommunityCategories
                .OrderBy(c => c.Id)
                .Select(c => c.Name)
                .ToListAsync();

            List<int> categoryIds = new List<int>();
            for (int i = 0; i < communitiesCategories.Count; i++)
            {
                categoryIds.Add(categories.IndexOf(communitiesCategories[i]) + 1);
            }

            var imageMediaExtensions = await context.MediaExtensions
                .Where(m => m.PermittedMediaType != null && m.PermittedMediaType.Type == "Image")
                .Select(m => m.Name)
                .AsNoTracking()
                .ToListAsync();

            // Защитная проверка: без пользователей и категорий сообщества создать нельзя
            if (!userIds.Any() || !categoryIds.Any())
            {
                return;
            }

            // 2. Инициализация MediaFiles из внешней директории (если их еще нет в базе)
            // Укажите ваш реальный путь. Буква '@' позволяет использовать обычные слэши.
            string communityFolderPath = @"C:\Users\RTK1\Pictures\Диплом\Сообщества";

            if (Directory.Exists(communityFolderPath) && !await context.MediaFiles.AnyAsync())
            {
                foreach (string category in communitiesCategories)
                {
                    // Фильтруем файлы по популярным расширениям изображений
                    var imageFiles = Directory.GetFiles(Path.Combine(communityFolderPath, category))
                        .Where(file => imageMediaExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                        .ToList();

                    if (imageFiles.Any())
                    {


                        var mediaFilesToSeed = new List<MediaFile>();
                        foreach (var filePath in imageFiles)
                        {
                            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
                            byte[] fileData = await File.ReadAllBytesAsync(filePath);
                            string imageFileContentType = GetContentType(fileExtension);
                            if (string.IsNullOrEmpty(fileExtension) || !imageMediaExtensions.Exists(n => n == fileExtension)) continue;
                            MediaExtension? dbfileExtension = await context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension);
                            if (dbfileExtension == null) continue;

                            mediaFilesToSeed.Add(new MediaFile
                            {
                                Name = Guid.NewGuid(),
                                ContentType = imageFileContentType,
                                ExtensionId = dbfileExtension.Id,
                                Data = fileData,
                                UploadDate = DateTime.UtcNow,
                            });
                        }

                        if (mediaFilesToSeed.Any())
                        {
                            await context.MediaFiles.AddRangeAsync(mediaFilesToSeed);
                            // Сохраняем файлы сейчас, чтобы база данных сгенерировала для них Id
                            await context.SaveChangesAsync();
                        }

                    }
                }

            }

            // 3. Проверяем, нужно ли генерировать сообщества
            if (await context.Communities.AnyAsync())
            {
                return;
            }

            // Получаем свежий список ID сохраненных картинок
            var avatarIds = await context.MediaFiles.Select(m => m.Id).ToListAsync();
            var communities = new List<Community>();



            // Генерируем 21 сообщество
            for (int i = 0; i < 21; i++)
            {
                string randomUserId = userIds[i % userIds.Count];
                int randomCategoryId = categoryIds[i % categoryIds.Count];

                // Если картинки нашлись и успешно добавились — берем ID, иначе — null
                int? randomAvatarId = avatarIds.Any() ? avatarIds[i % avatarIds.Count] : null;

                communities.Add(new Community
                {
                    Name = nameBriefCommunities[i]["name"],
                    DefaultUserId = randomUserId,
                    CommunityCategoryId = randomCategoryId,
                    AvatarId = randomAvatarId,
                    BriefInformation = nameBriefCommunities[i]["brief"]
                });
            }
            communities.Shuffle();

            await context.Communities.AddRangeAsync(communities);
            await context.SaveChangesAsync();


            // Генерируем записи
            // 5. Инициализация MediaFiles для ПОСТОВ (сохраняем их в базу первыми)
            string postImagesFolderPath = @"C:\Users\RTK1\Pictures\Диплом\Записи";
            var postMediaFilesPool = new List<MediaFile>();

            if (Directory.Exists(postImagesFolderPath))
            {

                foreach (string category in communitiesCategories)
                {
                    // Фильтруем файлы по популярным расширениям изображений
                    var postFiles = Directory.GetFiles(Path.Combine(postImagesFolderPath, category))
                        .Where(file => imageMediaExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                        .ToList();

                    if (postFiles.Any())
                    {


                        var mediaFilesToSeed = new List<MediaFile>();
                        foreach (var filePath in postFiles)
                        {
                            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
                            byte[] fileData = await File.ReadAllBytesAsync(filePath);
                            string imageFileContentType = GetContentType(fileExtension);
                            if (string.IsNullOrEmpty(fileExtension) || !imageMediaExtensions.Exists(n => n == fileExtension)) continue;
                            MediaExtension? dbfileExtension = await context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension);
                            if (dbfileExtension == null) continue;

                            postMediaFilesPool.Add(new MediaFile
                            {
                                Name = Guid.NewGuid(),
                                ContentType = imageFileContentType,
                                ExtensionId = dbfileExtension.Id,
                                Data = fileData,
                                UploadDate = DateTime.UtcNow,
                            });
                        }

                        if (postMediaFilesPool.Any())
                        {
                            await context.MediaFiles.AddRangeAsync(postMediaFilesPool);
                            // Сохраняем файлы сейчас, чтобы база данных сгенерировала для них Id
                            await context.SaveChangesAsync();
                        }

                    }
                }
            }

            // 6. Инициализация ПОСТОВ из CSV и привязка к ним картинок по 1-2 штуки
            string csvFilePath = @"F:\MyFiles\lenta-ru-news-recommend-data1.csv";

            if (File.Exists(csvFilePath) && !await context.Set<Post>().AnyAsync())
            {
                var generatedCommunities = await context.Communities
                    .Select(c => new { c.Id, c.DefaultUserId })
                    .ToListAsync();

                if (generatedCommunities.Any())
                {
                    var postsToSeed = new List<Post>();
                    var csvLines = await File.ReadAllLinesAsync(csvFilePath);

                    var rnd = new Random();
                    int currentFileIndex = 0; // Индекс для отслеживания остатка картинок в пуле
                    int postIndex = 0;

                    foreach (var line in csvLines)
                    {
                        string bodyText = line.Trim('"', ' ', '\t', '\r', '\n');
                        if (string.IsNullOrWhiteSpace(bodyText)) continue;

                        var targetCommunity = generatedCommunities[postIndex % generatedCommunities.Count];

                        // Создаем пост
                        var newPost = new Post
                        {
                            Body = bodyText.Length > 10000 ? bodyText.Substring(0, 10000) : bodyText,
                            PublicDate = DateTime.UtcNow,
                            CommunityId = targetCommunity.Id,
                            UserId = targetCommunity.DefaultUserId,
                            MediaFiles = new List<MediaFile>() // Инициализируем коллекцию для картинок
                        };

                        // Если картинки в пуле еще остались, прикрепляем 1 или 2 штуки
                        if (currentFileIndex < postMediaFilesPool.Count)
                        {
                            // Случайно выбираем количество: 1 или 2 (но не больше, чем осталось в пуле)
                            int imagesToAttach = rnd.Next(1, 3);
                            int remainingImages = postMediaFilesPool.Count - currentFileIndex;
                            int finalCount = Math.Min(imagesToAttach, remainingImages);

                            for (int j = 0; j < finalCount; j++)
                            {
                                newPost.MediaFiles.Add(postMediaFilesPool[currentFileIndex]);
                                currentFileIndex++; // Сдвигаем указатель, так как картинка "потрачена"
                            }
                        }

                        postsToSeed.Add(newPost);
                        postIndex++;
                    }

                    if (postsToSeed.Any())
                    {
                        await context.Set<Post>().AddRangeAsync(postsToSeed);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        private static string GetContentType(string extension)
        {
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                //".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }

        private static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
