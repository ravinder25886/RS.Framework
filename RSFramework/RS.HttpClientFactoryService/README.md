{"variant":"standard","id":"52123","title":"HttpClientFactoryService README"}

\# HttpClientFactoryService



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

\- Works with .NET 6, 7, 8+



---



\## Installation



Install via NuGet (example package name `comming soon..`):



```bash

dotnet add package comming soon..

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

&nbsp;   public int UserId { get; set; }

&nbsp;   public int Id { get; set; }

&nbsp;   public string Title { get; set; }

&nbsp;   public string Body { get; set; }

}

```



---



\### Basic GET Example



```csharp

public class PostsController : ControllerBase

{

&nbsp;   private readonly IHttpClientFactoryService \_httpService;



&nbsp;   public PostsController(IHttpClientFactoryService httpService)

&nbsp;   {

&nbsp;       \_httpService = httpService;

&nbsp;   }



&nbsp;   \[HttpGet("posts")]

&nbsp;   public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)

&nbsp;   {

&nbsp;       var result = await \_httpService.GetAsync<List<Post>>(

&nbsp;           uri: "posts",

&nbsp;           clientName: "JsonPlaceholder",

&nbsp;           cancellationToken: cancellationToken

&nbsp;       );



&nbsp;       return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);

&nbsp;   }

}

```



---



\### POST Example (JSON from C# model)



```csharp

var newPost = new Post { UserId = 1, Title = "Hello", Body = "World" };

var postResult = await \_httpService.PostAsync<Post>(

&nbsp;   uri: "posts",

&nbsp;   requestJsonData: JsonSerializer.Serialize(newPost),

&nbsp;   clientName: "JsonPlaceholder",

&nbsp;   cancellationToken: cancellationToken

);

```



---



\### CancellationToken Support



You can pass a `CancellationToken` to cancel requests:



```csharp

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

var result = await \_httpService.GetAsync<List<Post>>(

&nbsp;   "posts",

&nbsp;   clientName: "JsonPlaceholder",

&nbsp;   cancellationToken: cts.Token

);

```



---



\## Advanced Features



\- \*\*Multipart POST\*\*:



```csharp

var multipartContent = new MultipartFormDataContent();

multipartContent.Add(new StringContent("John"), "name");

var response = await \_httpService.PostMultipartAsync<HttpResult>(

&nbsp;   "users/upload",

&nbsp;   multipartContent,

&nbsp;   clientName: "MyApi",

&nbsp;   cancellationToken: cancellationToken

);

```



\- \*\*Form URL-encoded POST\*\*:



```csharp

var formData = new Dictionary<string, string>

{

&nbsp;   { "username", "john" },

&nbsp;   { "password", "secret" }

};

var result = await \_httpService.PostFormAsync<HttpResult>(

&nbsp;   "login",

&nbsp;   formData,

&nbsp;   clientName: "MyApi",

&nbsp;   cancellationToken: cancellationToken

);

```



---



\## Why use `HttpClientFactoryService`?



\- Reduces boilerplate code for common HTTP operations

\- Supports structured responses (`HttpResult<T>`) with status and messages

\- Works seamlessly with DI and `IHttpClientFactory`

\- Optional cancellation support

\- Consistent API for GET, POST, PUT, PATCH, DELETE, form, and multipart requests



---



\## License



MIT License



