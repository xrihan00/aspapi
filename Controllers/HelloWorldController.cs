using Microsoft.AspNetCore.Mvc;

namespace aspapi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase
{
    private readonly ILogger<HelloWorldController> _logger;

    public HelloWorldController(ILogger<HelloWorldController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetHelloWorld")]
    public IActionResult GetHelloWorld()
    {
        return Ok("Hello, world!");
    }
}
