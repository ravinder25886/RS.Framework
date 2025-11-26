namespace RS.HttpClientFactoryService;
public interface IHttpClientFactoryService
{
    /// <summary>
    /// Optional HttpClient timeout in minutes.
    /// If not provided, named HttpClient timeout configuration is used.
    /// </summary>
    public int HttpClientTimeout { get; set; }

    // ============================================================
    // GET
    // ============================================================

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> GetAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> GetAsync<T>(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResponseMessage> GetRawAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResponseMessage> representing the HTTP response.</returns>
    public Task<HttpResponseMessage> GetRawAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<byte[]> GetByteArrayAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<byte[]> representing the HTTP response.</returns>
    public Task<byte[]> GetByteArrayAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<T> GetObjectAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<T> representing the HTTP response.</returns>
    public Task<T> GetObjectAsync<T>(string url);

    // ============================================================
    // DELETE
    // ============================================================

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult> DeleteAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult> representing the HTTP response.</returns>
    public Task<HttpResult> DeleteAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResponseMessage> DeleteRawAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResponseMessage> representing the HTTP response.</returns>
    public Task<HttpResponseMessage> DeleteRawAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    // ============================================================
    // POST (JSON)
    // ============================================================

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> PostAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> PostAsync<T>(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult> PostAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult> representing the HTTP response.</returns>
    public Task<HttpResult> PostAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResponseMessage> PostRawAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResponseMessage> representing the HTTP response.</returns>
    public Task<HttpResponseMessage> PostRawAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    // ============================================================
    // POST (Multipart)
    // ============================================================

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> PostMultipartAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> PostMultipartAsync<T>(
        string uri, MultipartFormDataContent multipartForm, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResponseMessage> PostMultipartRawAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResponseMessage> representing the HTTP response.</returns>
    public Task<HttpResponseMessage> PostMultipartRawAsync(
        string uri, MultipartFormDataContent multipartForm, string? authHeaderName = null, string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    // ============================================================
    // PUT
    // ============================================================

    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> PutAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> PutAsync<T>(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    /// <summary>
    /// Executes HTTP operation: Task<HttpResponseMessage> PutRawAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResponseMessage> representing the HTTP response.</returns>
    public Task<HttpResponseMessage> PutRawAsync(
        string uri, string? authHeaderName = null, string? xAuthToken = null,
        string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

    // ============================================================
    // POST (Form URL Encoded)
    // ============================================================
    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> PostFormAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> PostFormAsync<T>(
        string uri,
        Dictionary<string, string> payload,
        string? authHeaderName = null,
        string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");
    /// <summary>
    /// Executes HTTP operation: Task<HttpResult> PostFormAsync.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult> representing the HTTP response.</returns>
    public Task<HttpResult> PostFormAsync(
        string uri,
        Dictionary<string, string> payload,
        string? authHeaderName = null,
        string? xAuthToken = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");


    // ============================================================
    // PATCH
    // ============================================================
    /// <summary>
    /// Executes HTTP operation: Task<HttpResult<T>> PatchAsync<T>.
    /// </summary>
    /// <param name="uri">Target URI for the request.</param>
    /// <param name="authHeaderName">Optional authorization header name.</param>
    /// <param name="xAuthToken">Optional authorization token.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <param name="clientName">Optional named HttpClient configuration.</param>
    /// <returns>Task<HttpResult<T>> representing the HTTP response.</returns>
    public Task<HttpResult<T>> PatchAsync<T>(
        string uri,

        string? authHeaderName = null,
        string? xAuthToken = null, string? requestJsonData = null,
        Dictionary<string, string>? headers = null,
        string clientName = "");

}

