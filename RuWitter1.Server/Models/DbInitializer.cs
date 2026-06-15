using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Services;
using System;
using System.IO;
using System.Text;


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
            "Наука и техника",
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

            foreach (var categoryName in categories) 
            {
                Console.WriteLine(categoryName);
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

            var communities = new List<Community>();
            int currentRandomUserId = 1;
            if (Directory.Exists(communityFolderPath) && !await context.MediaFiles.AnyAsync() && !await context.Communities.AnyAsync())
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

                        for(int i = 0; i < 3; i++) 
                        {
                            int? avatarId = mediaFilesToSeed.Any() ? mediaFilesToSeed[i % mediaFilesToSeed.Count].Id : null;
                            int categoryIndex = communitiesCategories.IndexOf(category);
                            int? categoryId = categoryIds[categoryIndex];
                            communities.Add(new Community
                            {
                                Name = nameBriefCommunities[i + categoryIndex * 3]["name"],
                                DefaultUserId = userIds[currentRandomUserId % userIds.Count],
                                CommunityCategoryId = categoryId,
                                AvatarId = avatarId,
                                BriefInformation = nameBriefCommunities[i + categoryIndex * 3]["brief"]
                            });
                            Console.WriteLine(userIds[currentRandomUserId % userIds.Count]);
                            currentRandomUserId++;
                        }

                    }
                }

                if (communities.Any())
                {
                    await context.Communities.AddRangeAsync(communities);
                    await context.SaveChangesAsync();
                }

            }

            // 3. Проверяем, нужно ли генерировать сообщества
            //if (await context.Communities.AnyAsync())
            //{
            //    return;
            //}

            



            // Генерируем 21 сообщество
            
            //foreach(int categoryId in categoryIds)
            //{
            //    string randomUserId = userIds[currentUserId % userIds.Count];

            //    communities.Add(new Community
            //    {
            //        Name = nameBriefCommunities[i]["name"],
            //        DefaultUserId = randomUserId,
            //        CommunityCategoryId = categoryId,
            //        AvatarId = randomAvatarId,
            //        BriefInformation = nameBriefCommunities[i]["brief"]
            //    });
            //}
            //communities.Shuffle();

            


            // Генерируем записи
            // 5. Инициализация MediaFiles для ПОСТОВ (сохраняем их в базу первыми)
            string postImagesFolderPath = @"C:\Users\RTK1\Pictures\Диплом\Записи";
            var postMediaFilesPool = new List<MediaFile>();
            string csvFilePath = @"C:\Users\RTK1\MyFiles\lenta-ru-news-recommend-data.csv";
            int textCountByCategory = 100;
            List<string> usedPostFiles = new List<string>();
            Console.OutputEncoding = Encoding.UTF8;
            if (File.Exists(csvFilePath) && !await context.Posts.AnyAsync())
            {
                var postsToSeed = new List<Post>();
                var csvLines = await File.ReadAllLinesAsync(csvFilePath, Encoding.UTF8);
                var rnd = new Random();
                foreach (string category in categories) 
                {
                    // Выгружаем только что созданные сообщества по текущей категории
                    var allCommunities = await context.Communities
                        .Where(c => c.CommunityCategory != null && c.CommunityCategory.Name == category)
                        .OrderBy(c => c.Id)
                        .ToListAsync();

                    Console.WriteLine(category);
                    foreach(var community in allCommunities) 
                    {
                        Console.WriteLine(community.Id);
                    }
                    
                    

                    // Чтобы картинки не перемешивались между категориями, сгруппируем посты по категориям.
                    // Для простоты — будем распределять строки из CSV по кругу, но картинки брать строго из папки нужной категории.
                    int postBeginIndex = categories.IndexOf(category) * textCountByCategory;
                    Console.WriteLine(postBeginIndex);
                    Console.WriteLine(postBeginIndex + textCountByCategory);

                    for(int i = postBeginIndex; i < postBeginIndex + textCountByCategory && i < csvLines.Length; i++)
                    {
                        Console.WriteLine(i);
                        string bodyText = csvLines[i].Trim('"', ' ', '\t', '\r', '\n');
                        if (string.IsNullOrWhiteSpace(bodyText)) continue;

                        // Берем текущее сообщество из базы
                        var targetCommunity = allCommunities[i % allCommunities.Count];

                        var newPost = new Post
                        {
                            Body = bodyText.Length > 10000 ? bodyText.Substring(0, 10000) : bodyText,
                            PublicDate = DateTime.UtcNow,
                            CommunityId = targetCommunity.Id,
                            UserId = targetCommunity.DefaultUserId,
                            MediaFiles = new List<MediaFile>()
                        };
                        //newPost.Body = newPost.Body.Replace("\0", "");

                        // Ищем папку с картинками постов для данной категории
                        string specificPostImagesPath = Path.Combine(postImagesFolderPath, category);

                        if (Directory.Exists(specificPostImagesPath))
                        {
                            var availableImages = Directory.GetFiles(specificPostImagesPath)
                                .Where(file => imageMediaExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                                .Where(file => !usedPostFiles.Contains(Path.GetFullPath(file)))
                                .ToList();

                            if (availableImages.Any())
                            {
                                // Выбираем случайную 1 или 2 картинки из папки этой категории
                                int imagesCountToAttach = rnd.Next(1, 3);

                                for (int j = 0; j < imagesCountToAttach; j++)
                                {
                                    // Случайно выбираем файл из доступных в этой папке
                                    string randomImagePath = availableImages[rnd.Next(availableImages.Count)];
                                    var fileExtension = Path.GetExtension(randomImagePath).ToLowerInvariant();
                                    MediaExtension? dbfileExtension = await context.MediaExtensions.AsNoTracking().SingleOrDefaultAsync(p => p.Name == fileExtension);

                                    if (dbfileExtension != null)
                                    {
                                        var postMediaFile = new MediaFile
                                        {
                                            Name = Guid.NewGuid(),
                                            ContentType = GetContentType(Path.GetExtension(fileExtension)),
                                            ExtensionId = dbfileExtension.Id,
                                            Data = await File.ReadAllBytesAsync(randomImagePath),
                                            UploadDate = DateTime.UtcNow
                                        };

                                        // Добавляем в коллекцию поста. EF Core сам сохранит медиафайл при сохранении поста!
                                        newPost.MediaFiles.Add(postMediaFile);
                                        usedPostFiles.Add(Path.GetFullPath(randomImagePath));
                                    }
                                }
                            }
                        }

                        postsToSeed.Add(newPost);
                    }
                }
                

                if (postsToSeed.Any())
                {
                    await context.Posts.AddRangeAsync(postsToSeed);
                    await context.SaveChangesAsync(); // Сохраняются и посты, и вложенные MediaFiles
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
