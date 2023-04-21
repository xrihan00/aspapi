using aspapi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DataContext _context;

    public DatabaseController(DataContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetDatabase")]
    public IActionResult Get()
    {
       // IEnumerable<IGrouping<string, int>> positiveNegative = _context.Data.GroupBy(d => d.Value).;
        List<int> currentData = _context.Data.OrderByDescending(d => d.Value).Select(d => d.Value).ToList();
        Dictionary<string,List<int>> positiveNegative = currentData.GroupBy(n => n < 0 ? "negative" : "positive").ToDictionary(g => g.Key, g => g.ToList());
        int sumInt = _context.Data.Select(d => d.Value).Sum();
        int sum = 0;
        if (currentData != null || currentData.Count>0)
        {            
            sum = currentData.Sum();
        }
        string s = string.Join("\n", positiveNegative["negative"]);
        //string s = string.Join("\n", positiveNegative["positive"]);
        //string s = string.Join("\n", currentData);
        return Ok(s);
    }

    [HttpPost]
    public IActionResult Post([FromBody] int value)
    {
        var data = new Data { Value = value };
        _context.Data.Add(data);
        _context.SaveChanges();
        return CreatedAtRoute("GetDatabase", new { id = data.Id }, data.Value);
    }

     [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] int value)
    {
        var data = _context.Data.SingleOrDefault(d => d.Id == id);
        if (data != null)
        {
            data.Value = value;
            _context.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var data = _context.Data.SingleOrDefault(d => d.Id == id);
        if (data != null)
        {
            _context.Data.Remove(data);
            _context.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

   
/*     public static List<int> mergeSortedLists(){
         List<int> a = new List<int>{1,3,5,8,9};
         List<int> b = new List<int>{0,2,6,7,9,10};

        List<int> merged = new List<int>();
        int i = 0,j = 0;
        while (i < a.Count && j < b.Count)
        {
            if(a[i]<b[j]){
                merged.Add(a[i]);
                i++;
            }
            else{
                merged.Add(b[j]);
                j++;
            }            
        }
        while(i < a.Count){
            merged.Add(a[i]);
            i++;
        }
        while(j < b.Count){
            merged.Add(b[j]);
            j++;
        }
        return merged;
    } */
}
