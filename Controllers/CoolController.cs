using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class CoolController : ControllerBase
{
    private readonly ISession _session;
    private const string DataKey = "Data";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CoolController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _session = _httpContextAccessor.HttpContext.Session;
    }

    [HttpGet("/cool", Name = "GetCool")]
    public IActionResult Get()
    {
        List<int> currentData = _session.GetObjectFromJson<List<int>>(DataKey) ?? new List<int>();
        int sum = 0;
        if (currentData != null || currentData.Count>0)
        {            
            sum = currentData.Sum();
        }
           
        return Ok(sum);
    }

    [HttpPost]
    public IActionResult Post([FromBody] int data)
    {
        List<int> currentData = _session.GetObjectFromJson<List<int>>(DataKey) ?? new List<int>();
        currentData.Add(data);
        _session.SetObjectAsJson(DataKey, currentData);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] int data)
    {
        List<int> currentData = _session.GetObjectFromJson<List<int>>(DataKey);
        if (currentData != null && id >= 0 && id < currentData.Count)
        {
            currentData[id] = data;
            _session.SetObjectAsJson(DataKey, currentData);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        List<string> currentData = _session.GetObjectFromJson<List<string>>(DataKey);
        if (currentData != null && id >= 0 && id < currentData.Count)
        {
            currentData.RemoveAt(id);
            _session.SetObjectAsJson(DataKey, currentData);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
public static class SessionExtensions
{
    public static void SetObjectAsJson<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        string value = session.GetString(key);
        if (value == null)
        {
            return default;
        }
        else if (typeof(T) == typeof(int))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        else
        {
            // You can add more type handling here if needed
            throw new ArgumentException($"Unsupported type '{typeof(T).FullName}'");
        }
    }
}

