using System.Net;
using System.Text.Json;

namespace OSMParserLocation
{
    internal class GetGeoJson
    {
        private HttpClient _httpClient;

        public GetGeoJson()
        {
            _httpClient = new HttpClient();
        }

        public async Task Run()
        {
            await GetDataJson();
        }

        private async Task GetDataJson()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                string address = "Москва";
                var response = await _httpClient.GetAsync($"https://nominatim.openstreetmap.org/search?format=json&q={address}&polygon_geojson=1");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(content);
                    JsonElement root = doc.RootElement;

                    // Выводим GeoJSON
                    Console.WriteLine(root[0].GetProperty("geojson"));
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}
