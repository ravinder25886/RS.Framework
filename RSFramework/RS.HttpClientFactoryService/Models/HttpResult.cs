using System.Net;

namespace RS.HttpClientFactoryService.Models;

public class HttpResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = default!;
    public HttpStatusCode StatusCode { get; set; }
    public HttpStatusCode HttpStatusCode { get; internal set; }
}

public class HttpResult<T> : HttpResult
{
    public T Data { get; set; } = default!;
}
