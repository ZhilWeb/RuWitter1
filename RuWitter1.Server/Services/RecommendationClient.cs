using RuWitter1.Server.Models;
using System.Text;
using System.Text.Json;

namespace RuWitter1.Server.Services
{
    public class RecommendationClient
    {
        private readonly HttpClient _httpClient;


        public RecommendationClient(
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> GetPostReccomends(List<int> communityPostsLikesIds,
            List<int> communityPostWatchesIds, List<string> postTexts, CancellationToken token = default)
        {
            var payload = JsonSerializer.Serialize(
                        new
                        {
                            likedIndices = communityPostsLikesIds,
                            likedTexts = postTexts
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

                    "http://127.0.0.1:8000/ruwrecom/",

                    content,
                    token
                );

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<RecommendationResponse>(cancellationToken: token);
            if (data == null)
            {
                return [];
            }
            return data.Recommendations;
        }
    }
}
