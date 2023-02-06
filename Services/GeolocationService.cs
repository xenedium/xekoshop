using Newtonsoft.Json;
using xekoshop.Data;
using xekoshop.Interfaces;

namespace xekoshop.Services;

public class GeolocationService : IGeolocationService
{
    public string ApiUrl { get; set; }
    public HttpClient Client { get; set; }
    
    public GeolocationService()
    {
        ApiUrl = "http://ip-api.com/json/";
        Client = new HttpClient();
    }
    
    public async Task<GeolocationResult?> GetGeolocation(string ip)
    {
        var response = await Client.GetAsync(ApiUrl + ip);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GeolocationResult>(content);
    }
}