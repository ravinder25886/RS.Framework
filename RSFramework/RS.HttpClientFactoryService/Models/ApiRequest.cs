namespace RS.HttpClientFactoryService.Models;
public class ApiRequest
{
    public string Uri { get; set; } = string.Empty;
    public object? Payload { get; set; }
    public string? AuthHeaderName { get; set; }
    public string? AuthToken { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public string ClientName { get; set; } = string.Empty;
}
