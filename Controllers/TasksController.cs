using borntocode_backend.Database;
using borntocode_backend.Database.Models;
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
            .AsNoTracking()
            .Include(t => t.Author)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Author = t.Author.Name,
                AuthorId = t.AuthorId,
                ShortDescription = t.ShortDescription!,
                FullDescription = t.FullDescription!,
                CodeExample = t.CodeExample!,
                CreationDate = t.CreationDate,
                Languages = t.Languages ?? new List<bool> {false, false, false},
                Likes = t.Likes,
                Views = t.Views,
                HtmlStruct = JsonConvert.DeserializeObject<TaskHtmlStructDto>(t.Struct)!
            })
            .ToListAsync();
    }
    
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
    
    [HttpGet("get/user/{name}")]
    public async Task<ActionResult<List<TaskDto>>> GetUserTasks(string name)
    {
        await using var context = new ApplicationContext();

        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Name == name);
        if (user == null)
            return NotFound();

        return user.Tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Author = t.Author.Name,
            AuthorId = t.AuthorId,
            ShortDescription = t.ShortDescription!,
            FullDescription = t.FullDescription!,
            CodeExample = t.CodeExample!,
            CreationDate = t.CreationDate,
            Languages = t.Languages ?? new List<bool> {false, false, false},
            Likes = t.Likes,
            Views = t.Views,
            HtmlStruct = JsonConvert.DeserializeObject<TaskHtmlStructDto>(t.Struct)!
        }).ToList();
    }
    
    [HttpGet("get/completed/user/{name}")]
    public async Task<ActionResult<List<TaskDto>>> GetUserCompletedTasks(string name)
    {
        await using var context = new ApplicationContext();

        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == name);
        if (user == null)
            return NotFound();

        var tasks = new List<CodeTask>();
        foreach (var solution in user.Solutions.Where(s => s.Completed))
        {
            var task = await context.Tasks
                .AsNoTracking()
                .Include(t => t.Author)
                .FirstOrDefaultAsync(t => t.Id == solution.TaskId);
            if (task == null)
                continue;
            
            tasks.Add(task);
        }

        if (tasks.Count == 0)
            return new List<TaskDto>();
        
        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Author = t.Author.Name,
            AuthorId = t.AuthorId,
            ShortDescription = t.ShortDescription!,
            FullDescription = t.FullDescription!,
            CodeExample = t.CodeExample!,
            CreationDate = t.CreationDate,
            Languages = t.Languages ?? new List<bool> {false, false, false},
            Likes = t.Likes,
            Views = t.Views,
            HtmlStruct = JsonConvert.DeserializeObject<TaskHtmlStructDto>(t.Struct)!
        }).ToList();
    }
}