using xekoshop.Data;

namespace xekoshop.Interfaces;

public interface IGeolocationService
{
    public HttpClient Client { get; set; }
    public string ApiUrl { get; set; }
    
    public Task<GeolocationResult?> GetGeolocation(string ip);
}