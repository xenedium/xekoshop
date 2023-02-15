using Newtonsoft.Json;

namespace xekoshop.Data;

public class GeolocationResult
{
    [JsonProperty("query")]
    public string Query { get; set; } = default!;
    [JsonProperty("status")]
    public string Status { get; set; } = default!;
    [JsonProperty("country")]
    public string Country { get; set; } = default!;
    [JsonProperty("countryCode")]
    public string CountryCode { get; set; } = default!;
    [JsonProperty("region")]
    public string Region { get; set; } = default!;
    [JsonProperty("regionName")]
    public string RegionName { get; set; } = default!;
    [JsonProperty("city")]
    public string City { get; set; } = default!;
    [JsonProperty("zip")]
    public string Zip { get; set; } = default!;
    [JsonProperty("lat")]
    public double Lat { get; set; }
    [JsonProperty("lon")]
    public double Lon { get; set; }
    [JsonProperty("timezone")]
    public string Timezone { get; set; } = default!;
    [JsonProperty("isp")]
    public string Isp { get; set; } = default!;
    [JsonProperty("org")]
    public string Org { get; set; } = default!;
    [JsonProperty("as")]
    public string As { get; set; } = default!;
}