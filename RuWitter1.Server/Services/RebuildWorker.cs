using RuWitter1.Server.Models;
using System.Text;
using System.Text.Json;

namespace RuWitter1.Server.Services
{
    public class RebuildWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly HttpClient _httpClient;


        public RebuildWorker(

            IServiceScopeFactory scopeFactory,

            HttpClient httpClient
        )
        {
            _scopeFactory = scopeFactory;

            _httpClient = httpClient;
        }

        


        protected override async Task ExecuteAsync(

            CancellationToken stoppingToken
        )
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var db = scope.ServiceProvider
                        .GetRequiredService<PostContext>();


                    // =====================================
                    // Load posts from PostgreSQL
                    // =====================================

                    var posts = db.Posts
                        .Where(p => p.CommunityId != null)
                        .Select(p => new { 
                            id = p.Id,
                            text = p.Body
                        })
                        .ToList();


                    // =====================================
                    // JSON payload
                    // =====================================

                    var payload = JsonSerializer.Serialize(
                        new
                        {
                            posts = posts
                        }
                    );

                    var content = new StringContent(

                        payload,

                        Encoding.UTF8,

                        "application/json"
                    );


                    // =====================================
                    // Trigger rebuild
                    // =====================================

                    var response = await _httpClient
                        .PostAsync(

                            "http://127.0.0.1:8000/ruwrecom-rebuild/",

                            content,

                            stoppingToken
                        );

                    Console.WriteLine(
                        $"Rebuild status: {response.StatusCode}"
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }


                // =========================================
                // Periodic rebuild
                // =========================================

                await Task.Delay(

                    TimeSpan.FromMinutes(5),

                    stoppingToken
                );
            }
        }
    }
}
