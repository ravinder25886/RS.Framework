# HttpClientFactoryService



A reusable, flexible HTTP client service for .NET, built on top of `IHttpClientFactory`.  

Supports JSON, form data, multipart requests, GET/POST/PUT/PATCH/DELETE operations, and optional `CancellationToken`.



---



\## Features



\- Simplified HTTP calls with minimal code

\- Named `HttpClient` support

\- Automatic JSON serialization/deserialization

\- Form URL-encoded and multipart requests

\- Optional headers and authorization tokens

\- Handles response errors and returns structured `HttpResult<T>`

\- Supports cancellation via `CancellationToken`

\- Works with .NET 8+



---



\## Installation



Install via NuGet (example package name `RS.HttpClientFactoryService`):



```bash

dotnet add package RS.HttpClientFactoryService

```



---



\## Usage



\### Register in `Program.cs` / `Startup.cs`



```csharp

builder.Services.AddHttpClient();
OR
builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();

```



\### Define a model



```csharp

public class Post

{

public int UserId { get; set; }

public int Id { get; set; }

public string Title { get; set; }

public string Body { get; set; }

}

```



---



\### Basic GET Example



```csharp

public class PostsController : ControllerBase

{

private readonly IHttpClientFactoryService _httpService;



public PostsController(IHttpClientFactoryService httpService)

{

    _httpService = httpService;

}



[HttpGet("posts")]

public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)

{

    var result = await _httpService.GetAsync<List<Post>>(

        uri: "posts",

        clientName: "JsonPlaceholder",

        cancellationToken: cancellationToken

    );



    return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);

}

}

```



---



### POST Example (JSON from C# model)



```csharp

var newPost = new Post { UserId = 1, Title = "Hello", Body = "World" };

var postResult = await _httpService.PostAsync<Post>(

uri: "posts",

requestJsonData: JsonSerializer.Serialize(newPost),

clientName: "JsonPlaceholder",

cancellationToken: cancellationToken

);

```



---



### CancellationToken Support



You can pass a `CancellationToken` to cancel requests:



```csharp

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

var result = await _httpService.GetAsync<List<Post>>(

"posts",

clientName: "JsonPlaceholder",

cancellationToken: cts.Token

);

```



---



## Advanced Features



\- \*\*Multipart POST\*\*:



```csharp

var multipartContent = new MultipartFormDataContent();

multipartContent.Add(new StringContent("John"), "name");

var response = await _httpService.PostMultipartAsync<HttpResult>(

"users/upload",

multipartContent,

clientName: "MyApi",

cancellationToken: cancellationToken

);

```



\- \*\*Form URL-encoded POST\*\*:



```csharp

var formData = new Dictionary<string, string>

{

{ "username", "john" },

{ "password", "secret" }

};

var result = await _httpService.PostFormAsync<HttpResult>(

"login",

formData,

clientName: "MyApi",

cancellationToken: cancellationToken

);

```



---



## Why use `HttpClientFactoryService`?



\- Reduces boilerplate code for common HTTP operations

\- Supports structured responses (`HttpResult<T>`) with status and messages

\- Works seamlessly with DI and `IHttpClientFactory`

\- Optional cancellation support

\- Consistent API for GET, POST, PUT, PATCH, DELETE, form, and multipart requests



---

## Read more / Full Article

For detailed explanation, motivation, and examples — see the full blog post:  
[RS.HttpClientFactoryService — A Cleaner Way to Call APIs in .NET]  
(https://www.theravinder.com/blog/rs-httpclientfactoryservice-colon-and-hyphen-a-cleaner-way-to-call-apis-in--dot-net-10068)


\## License



MIT License



