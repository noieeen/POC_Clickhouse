using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceA.Models;


namespace ServiceA.Controllers.LoadTestController;

[ApiController]
[Route("api/load")]
public class LoadTestController : ControllerBase
{
    private static readonly HttpClient _httpClient = new HttpClient();

    [HttpGet("cpu")]
    public IActionResult StressCpu()
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < 5000) // 5 seconds CPU load
        {
            Math.Sqrt(new Random().Next());
        }

        return Ok("CPU load test completed");
    }

    [HttpGet("memory")]
    public IActionResult StressMemory()
    {
        var list = new List<byte[]>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(new byte[1024 * 1024]); // Allocate 1MB chunks
            Task.Delay(100).Wait();
        }

        return Ok("Memory load test completed");
    }

    [HttpGet("http")]
    public async Task<IActionResult> HttpLoadTest()
    {
        var tasks = new List<Task<HttpResponseMessage>>();
        for (int i = 0; i < 50; i++) // Simulate 50 concurrent requests
        {
            tasks.Add(_httpClient.GetAsync("https://example.com"));
        }

        await Task.WhenAll(tasks);
        return Ok("HTTP load test completed");
    }
}