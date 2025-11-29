using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using RS.HttpClientFactoryService;
using RS.HttpClientFactoryService.Models;
namespace HttpClientFactoryServiceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestApiController(IHttpClientFactoryService httpService) : ControllerBase
{
    private readonly IHttpClientFactoryService _httpService = httpService;
    private string _apiName = "JsonPlaceholder";
    // GET api/testapi/posts
    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts()
    {
        HttpResult<List<Post>> result = await _httpService.GetAsync<List<Post>>("posts",clientName: _apiName);
        return !result.IsSuccess ? BadRequest(result.Message) : Ok(result.Data);
    }

    // GET api/testapi/posts/1
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        try
        {
            Post post = await _httpService.GetObjectAsync<Post>($"posts/{id}", clientName: _apiName);
            return Ok(post);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    //POST api/testapi/posts
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] Post newPost)
    {

        CreatePostModel post = new CreatePostModel
        {
            UserId = 1,
            Title = "Sample Title",
            Body = "Body text goes here"
        };

        string jsonBody = JsonSerializer.Serialize(post);

        string authHeaderName = "Authorization";
        string token = "Bearer dummy-token";

        Dictionary<string, string> extraHeaders = new Dictionary<string, string>
    {
        { "X-Correlation-ID", Guid.NewGuid().ToString() },
        { "X-Request-Source", "DemoClient" }
    };

        var result = await _httpService.PostAsync(
            uri: "posts",
            authHeaderName: authHeaderName,
            xAuthToken: token,
            requestJsonData: jsonBody,
            headers: extraHeaders,
            clientName: _apiName
        );

        Console.WriteLine($"Status: {result.StatusCode}");
        Console.WriteLine($"Response: {result.IsSuccess}");
        return !result.IsSuccess ? BadRequest(result.Message) : CreatedAtAction(nameof(GetPost), new { id = result.IsSuccess }, result.IsSuccess);
    }

    // DELETE api/testapi/posts/1
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        HttpResult result = await _httpService.DeleteAsync($"posts/{id}", clientName: _apiName);
        return !result.IsSuccess ? BadRequest(result.Message) : NoContent();
    }


}

// Simple model for JSONPlaceholder post
public class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
}
public class CreatePostModel
{
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
}
