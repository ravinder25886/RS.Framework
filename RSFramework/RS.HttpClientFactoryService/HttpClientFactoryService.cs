using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using RS.HttpClientFactoryService.Models;

namespace RS.HttpClientFactoryService;

public class HttpClientFactoryService : IHttpClientFactoryService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions;
public int HttpClientTimeout { get; set; } = 0;

    public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    // ============================================================
    // Helper: Create HttpClient with headers
    // ============================================================
    private HttpClient CreateClient(string? authHeaderName, string? token, Dictionary<string, string>? headers, string clientName)
    {
        HttpClient client = string.IsNullOrWhiteSpace(clientName)
            ? _httpClientFactory.CreateClient()
            : _httpClientFactory.CreateClient(clientName);

        if (HttpClientTimeout > 0)
        {
            client.Timeout = TimeSpan.FromMinutes(HttpClientTimeout);
        }

        if (!string.IsNullOrWhiteSpace(authHeaderName) && !string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Add(authHeaderName, token);
        }

        if (headers != null)
        {
            foreach (var h in headers)
            {
                if (!client.DefaultRequestHeaders.Contains(h.Key))
                {
                    client.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
            }
        }

        if (!client.DefaultRequestHeaders.Accept.Any())
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        return client;
    }

    // ============================================================
    // Helper: Send Request
    // ============================================================
    private Task<HttpResponseMessage> SendBasic(HttpMethod method, string uri, HttpClient client, HttpContent? content)
    {
        HttpRequestMessage req = new HttpRequestMessage(method, uri) { Content = content };
        return client.SendAsync(req);
    }

    // ============================================================
    // Helper: Handle & Deserialize Response
    // ============================================================
    private async Task<HttpResult<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        string body = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new HttpResult<T>
            {
                IsSuccess = false,
                Message = "Item not found",
                HttpStatusCode = response.StatusCode
            };
        }
        if (!response.IsSuccessStatusCode)
        {
            return new HttpResult<T>
            {
                IsSuccess = false,
                Message = body,
                HttpStatusCode = response.StatusCode
            };
        }

        T? data = default;
        if (!string.IsNullOrWhiteSpace(body))
        {
            data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
        }

        return new HttpResult<T>
        {
            IsSuccess = true,
            Message = "SUCCESS",
            HttpStatusCode = response.StatusCode,
            Data = data
        };
    }

    // ============================================================
    // Helper: Generic JSON sender
    // ============================================================
    private async Task<HttpResult<T>> SendJsonAsync<T>(HttpMethod method, string uri, object? payload = null,
        string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);

        HttpContent? content = null;
        if (payload != null)
        {
            string json = payload is string s ? s : JsonSerializer.Serialize(payload, _jsonOptions);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response = await SendBasic(method, uri, client, content);
        return await HandleResponse<T>(response);
    }

    // ============================================================
    // GET
    // ============================================================
    public async Task<HttpResult<T>> GetAsync<T>(string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
        return await SendJsonAsync<T>(HttpMethod.Get, uri, null, authHeaderName, xAuthToken, headers, clientName);
    }

    public async Task<HttpResponseMessage> GetRawAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        return await SendBasic(HttpMethod.Get, uri, client, null);
    }

    public async Task<byte[]> GetByteArrayAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpResponseMessage response = await SendBasic(HttpMethod.Get, uri, client, null);
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<T> GetObjectAsync<T>(
     string url,
     string? authHeaderName = null,
     string? xAuthToken = null,
     Dictionary<string, string>? headers = null,
     string clientName = "")
    {
        return await SendJsonAsync<T>(HttpMethod.Get, url, null, authHeaderName, xAuthToken, headers, clientName)
       .ContinueWith(t => t.Result.Data!);
    }


    // ============================================================
    // DELETE
    // ============================================================
    public async Task<HttpResult> DeleteAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpResponseMessage response = await SendBasic(HttpMethod.Delete, uri, client, null);
        string body = await response.Content.ReadAsStringAsync();

        return new HttpResult
        {
            IsSuccess = response.IsSuccessStatusCode,
            Message = response.IsSuccessStatusCode ? "SUCCESS" : body,
            HttpStatusCode = response.StatusCode
        };
    }

    public async Task<HttpResponseMessage> DeleteRawAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        return await SendBasic(HttpMethod.Delete, uri, client, null);
    }

    // ============================================================
    // POST / PUT / PATCH JSON
    // ============================================================
    public Task<HttpResult<T>> PostAsync<T>(string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null, Dictionary<string, string>? headers = null, string clientName = "")
    {
        return SendJsonAsync<T>(HttpMethod.Post, uri, requestJsonData, authHeaderName, xAuthToken, headers, clientName);
    }
    public async Task<HttpResult> PostAsync(
    string uri,
    string? authHeaderName = null,
    string? xAuthToken = null,
    string? requestJsonData = null,
    Dictionary<string, string>? headers = null,
    string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);

        HttpContent? content = null;
        if (!string.IsNullOrWhiteSpace(requestJsonData))
        {
            content = new StringContent(requestJsonData, Encoding.UTF8, "application/json");
        }

        // Send the request
        HttpResponseMessage response = await SendBasic(HttpMethod.Post, uri, client, content);

        // Handle response
        return await HandleResponse<HttpResult>(response);
    }
    public Task<HttpResult<T>> PutAsync<T>(string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null, Dictionary<string, string>? headers = null, string clientName = "")
    {
        return SendJsonAsync<T>(HttpMethod.Put, uri, requestJsonData, authHeaderName, xAuthToken, headers, clientName);
    }

    public Task<HttpResult<T>> PatchAsync<T>(string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null, Dictionary<string, string>? headers = null, string clientName = "")
    {
        return SendJsonAsync<T>(new HttpMethod("PATCH"), uri, requestJsonData, authHeaderName, xAuthToken, headers, clientName);
    }

    // ============================================================
    // POST FormUrlEncoded
    // ============================================================
    public async Task<HttpResult<T>> PostFormAsync<T>(string uri, Dictionary<string, string> payload,
        string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
        if (payload == null || payload.Count == 0)
        {
            throw new ArgumentException("Payload cannot be null or empty.", nameof(payload));
        }

      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpContent? content = payload.Count > 0 ? new FormUrlEncodedContent(payload) : null;
        HttpResponseMessage response = await SendBasic(HttpMethod.Post, uri, client, content);
        return await HandleResponse<T>(response);
    }

    public async Task<HttpResult> PostFormAsync(string uri, Dictionary<string, string> payload,
        string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
        if (payload == null || payload.Count == 0)
        {
            throw new ArgumentException("Payload cannot be null or empty.", nameof(payload));
        }

      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpContent? content = payload.Count > 0 ? new FormUrlEncodedContent(payload) : null;
        HttpResponseMessage response = await SendBasic(HttpMethod.Post, uri, client, content);
        return await HandleResponse<HttpResult>(response);
    }

    // ============================================================
    // Multipart POST
    // ============================================================
    public async Task<HttpResult<T>> PostMultipartAsync<T>(string uri, MultipartFormDataContent multipartForm,
        string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpResponseMessage response = await SendBasic(HttpMethod.Post, uri, client, multipartForm);
        return await HandleResponse<T>(response);
    }

    public async Task<HttpResponseMessage> PostMultipartRawAsync(string uri, MultipartFormDataContent multipartForm,
        string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        return await SendBasic(HttpMethod.Post, uri, client, multipartForm);
    }

    // ============================================================
    // Raw Requests
    // ============================================================
    public async Task<HttpResponseMessage> PostRawAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null, Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpContent content = new StringContent(requestJsonData ?? "", Encoding.UTF8, "application/json");
        return await SendBasic(HttpMethod.Post, uri, client, content);
    }

    public async Task<HttpResponseMessage> PutRawAsync(string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null, Dictionary<string, string>? headers = null, string clientName = "")
    {
      HttpClient client = CreateClient(authHeaderName, xAuthToken, headers, clientName);
        HttpContent content = new StringContent(requestJsonData ?? "", Encoding.UTF8, "application/json");
        return await SendBasic(HttpMethod.Put, uri, client, content);
    }
    
}
