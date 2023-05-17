using borntocode_backend.Database;
using borntocode_backend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace borntocode_backend.Controllers;

[ApiController]
[Route("tasks")]
public class TasksController : Controller
{
    [HttpGet("getAll")]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        await using var context = new ApplicationContext();

        return await context.Tasks
            .Include(t => t.Author)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Author = t.Author.Name,
                ShortDescription = t.ShortDescription!,
                FullDescription = t.FullDescription!,
                CodeExample = t.CodeExample!,
                HtmlStruct = JsonConvert.DeserializeObject<TaskHtmlStructDto>(t.Struct)!
            })
            .ToListAsync();
    }

    [Authorize]
    [HttpGet("get/{id:int}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        await using var context = new ApplicationContext();

        var task = await context.Tasks
            .Include(t => t.Author)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (task == null)
            return NotFound();

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Author = task.Author.Name,
            ShortDescription = task.ShortDescription!,
            FullDescription = task.FullDescription!,
            CodeExample = task.CodeExample!,
            HtmlStruct = JsonConvert.DeserializeObject<TaskHtmlStructDto>(task.Struct)!
        };
    }

    [Authorize]
    [HttpGet("get/user/{name}")]
    public async Task<ActionResult<List<TaskDto>>> GetUserTasks(string name)
    {
        await using var context = new ApplicationContext();

        var user = await context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Name == name);
        if (user == null)
            return NotFound();

        return user.Tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Author = t.Author.Name,
            ShortDescription = t.ShortDescription!,
            FullDescription = t.FullDescription!,
        }).ToList();
    }
}