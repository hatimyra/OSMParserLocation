using System.Net;
using System.Text.Json;
using System.Xml;

namespace OSMParserLocation
{
    public class GetGeoJson
    {
        private HttpClient _httpClient = new HttpClient();
        public JsonDocument? GeoJsonData { get; private set; }

        public async Task GetDataJson(string address)
        {
            
            string content = string.Empty;
            try
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                //https://nominatim.openstreetmap.org/search?format=json&q=Московская область Авдотьино&polygon_geojson=1
                var response = await _httpClient.GetAsync($"https://nominatim.openstreetmap.org/search?format=json&q={address}&polygon_geojson=1");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = await response.Content.ReadAsStringAsync();
                    JsonDocument GeoJsonData = JsonDocument.Parse(content);
                    JsonElement jsonElement = GeoJsonData.RootElement;

                    var coord = jsonElement[0].GetProperty("geojson").GetProperty("coordinates").EnumerateArray().ToList();

                    foreach (var item in coord)
                    {
                        foreach (var item2 in item.EnumerateArray().ToList())
                        {
                            foreach (var item3 in item2.EnumerateArray().ToList())
                                Console.WriteLine("{0}", item3);
                        }
                    }

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

        private void CreatePoligon() { }
        private void CreateMultiPoligon() { }
        private List<List<List<double>>> CoordinatestoDoubleType () { return  }

    }
}


/*using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        var geoJson = new GeoJson
        {
            Type = "FeatureCollection",
            Metadata = new Metadata
            {
                Name = "карта новая",
                Creator = "Yandex Map Constructor"
            },
            Features = new List<Feature>()
        };

        // Пример добавления многоугольника
        AddPolygon(geoJson, 0, "НОГИНСК",
            new List<List<double>>
            {
                new List<double> { 38.44235773016348, 55.85343429537219 },
                new List<double> { 38.43373174597159, 55.839044156848956 },
                new List<double> { 38.46521721096333, 55.83567857732218 },
                new List<double> { 38.49015102596578, 55.839252777406536 },
                new List<double> { 38.4909664175063, 55.85202543063928 },
                new List<double> { 38.467622151498354, 55.85619153365231 },
                new List<double> { 38.46131359589534, 55.85302928456875 },
                new List<double> { 38.4601548816009, 55.853946604805685 },
                new List<double> { 38.44235773016348, 55.85343429537219 }
            }, "#ed4543", 0.6, "#ed4543", "5", 0.9);

        // Пример добавления многоугольника
        AddMultiPolygon(geoJson, 1, "Благовещение",
            new List<List<List<double>>>
            {
                new List<List<double>>
                {
                    new List<double> { 38.448567738656585, 55.84419292886431 },
                    new List<double> { 38.45165764344172, 55.83982229153154 },
                    new List<double> { 38.4528592730804, 55.83989473705585 },
                    new List<double> { 38.45423256409602, 55.840232814376925 },
                    new List<double> { 38.45899616730646, 55.84151265177803 },
                    new List<double> { 38.45612083924246, 55.83866314469957 },
                    new List<double> { 38.467965474252225, 55.83897708372486 },
                    new List<double> { 38.46985374939871, 55.84177827310019 },
                    new List<double> { 38.465218892220975, 55.843806594058066 },
                    new List<double> { 38.47225700867606, 55.8432995237819 },
                    new List<double> { 38.46787964356375, 55.84846650032263 },
                    new List<double> { 38.46371685517264, 55.85213608343017 },
                    new List<double> { 38.455605855111614, 55.849794350166235 },
                    new List<double> { 38.44788109314872, 55.84433780340098 },
                    new List<double> { 38.44843899262382, 55.84448267741465 },
                    new List<double> { 38.448567738656585, 55.84419292886431 }
                },
                // Добавьте дополнительные многоугольники, если необходимо
                new List<List<double>>
                {
                    new List<double> { 38.450, 55.840 },
                    new List<double> { 38.455, 55.845 },
                    new List<double> { 38.460, 55.840 },
                    new List<double> { 38.450, 55.840 } // Замыкание
                }
            }, "#ed4543", 0.6, "#ed4543", "5", 0.9);

        string json = JsonConvert.SerializeObject(geoJson, Formatting.Indented);
        Console.WriteLine(json);
    }

    public static void AddPolygon(GeoJson geoJson, int id, string description, List<List<double>> coordinates, string fill, double fillOpacity, string stroke, string strokeWidth, double strokeOpacity)
    {
        var feature = new Feature
        {
            Id = id,
            Geometry = new Geometry
            {
                Type = "Polygon",
                Coordinates = new List<List<List<double>>> { coordinates }
            },
            Properties = new Properties
            {
                Description = description,
                Fill = fill,
                FillOpacity = fillOpacity,
                Stroke = stroke,
                StrokeWidth = strokeWidth,
                StrokeOpacity = strokeOpacity
            }
        };

        geoJson.Features.Add(feature);
    }

    public static void AddMultiPolygon(GeoJson geoJson, int id, string description, List<List<List<double>>> coordinates, string fill, double fillOpacity, string stroke, string strokeWidth, double strokeOpacity)
    {
        var feature = new Feature
        {
            Id = id,
            Geometry = new Geometry
            {
                Type = "MultiPolygon",
                Coordinates = coordinates
            },
            Properties = new Properties
            {
                Description = description,
                Fill = fill,
                FillOpacity = fillOpacity,
                Stroke = stroke,
                StrokeWidth = strokeWidth,
                StrokeOpacity = strokeOpacity
            }
        };

        geoJson.Features.Add(feature);
    }
}

public class GeoJson
{
    public string Type { get; set; }
    public Metadata Metadata { get; set; }
    public List<Feature> Features { get; set; }
}

public class Metadata
{
    public string Name { get; set; }
    public string Creator { get; set; }
}

public class Feature
{
    public int Id { get; set; }
    public Geometry Geometry { get; set; }
    public Properties Properties { get; set; }
}

public class Geometry
{
    public string Type { get; set; }
    public List<List<List<double>>> Coordinates { get; set; }
}

public class Properties
{
    public string Description { get; set; }
    public string Fill { get; set; }
    public double FillOpacity { get; set; }
    public string Stroke { get; set; }
    public string StrokeWidth { get; set; }
    public double StrokeOpacity { get; set; }
}
*/