using System.Security.Claims;
using borntocode_backend.Database;
using borntocode_backend.Database.Models;
using borntocode_backend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace borntocode_backend.Controllers;

[ApiController]
[Route("solutions")]
public class SolutionController : Controller
{
    [HttpGet("{name}/getAll")]
    public async Task<ActionResult<List<SolutionDto>>> GetUserSolutions(string name)
    {
        await using var context = new ApplicationContext();
        
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == name);
        if (user == null)
            return NotFound();
        
        var solutionDtos = new List<SolutionDto>();
        foreach (var solution in user.Solutions)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == solution.TaskId);
            if (task == null)
                continue;
            
            var solutionDto = new SolutionDto
            {
                Id = solution.Id,
                TaskId = solution.TaskId,
                UserId = solution.UserId,
                CreatedAt = solution.CreatedAt,
                Html = solution.Html,
                Css = solution.Css,
                Js = solution.Js,
                Completed = solution.Completed,
                Title = task.Title
            };
            
            solutionDtos.Add(solutionDto);
        }

        return solutionDtos;
    }
    
    [Authorize]
    [HttpPut("add")]
    public async Task<ActionResult> AddSolutionToUser([FromBody] SolutionDto solutionDto)
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest();

        await using var context = new ApplicationContext();

        var user = await context.Users
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == userName);
        if (user == null)
            return NotFound("Такой пользователь не найден");

        var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == solutionDto.TaskId);
        if (task == null)
            return NotFound("Такой задачи не существует");

        var exists = user.Solutions.ToList().Exists(s => s.TaskId == solutionDto.TaskId);
        if (exists)
            return BadRequest("Для данного пользователя такая задача уже добавлена");
        
        var solution = new Solution
        {
            TaskId = task.Id,
            UserId = user.Id,
            CreatedAt = solutionDto.CreatedAt,
            Html = solutionDto.Html,
            Css = solutionDto.Css,
            Js = solutionDto.Js
        };

        await context.Solutions.AddAsync(solution);
        
        await context.SaveChangesAsync();

        solution.Task = task;
        solution.User = user;
        
        user.Solutions.Add(solution);

        return Ok();
    }

    [Authorize]
    [HttpGet("isAdded/{taskId:int}")]
    public async Task<ActionResult> IsSolutionAdded(int taskId)
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest();
        
        await using var context = new ApplicationContext();

        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == userName);
        if (user == null)
            return NotFound("Такой пользователь не найден");

        var exists = user.Solutions.ToList().Exists(s => s.TaskId == taskId);

        return Ok(exists);
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<ActionResult> UpdateSolution([FromBody] SolutionDto solutionDto)
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest();
        
        await using var context = new ApplicationContext();

        var user = await context.Users
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == userName);
        if (user == null)
            return NotFound("Такой пользователь не найден");

        var solution = user.Solutions.FirstOrDefault(s => s.TaskId == solutionDto.TaskId);
        if (solution == null)
            return NotFound("Такой задачи у пользователя не найдено");

        if (!solution.Completed)
            solution.Completed = solutionDto.Completed;
        
        solution.Html = solutionDto.Html;
        solution.Css = solutionDto.Css;
        solution.Js = solutionDto.Js;

        await context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("{name}/get/{taskId:int}")]
    public async Task<ActionResult<SolutionDto>> GetSolution(string name, int taskId)
    {
        await using var context = new ApplicationContext();
        
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Solutions)
            .FirstOrDefaultAsync(u => u.Name == name);
        if (user == null)
            return NotFound("Такой пользователь не найден");

        var solution = user.Solutions.FirstOrDefault(s => s.TaskId == taskId);
        if (solution == null)
            return NotFound("Такой задачи у пользователя нет");

        return new SolutionDto
        {
            TaskId = taskId,
            UserId = user.Id,
            CreatedAt = solution.CreatedAt,
            Html = solution.Html,
            Css = solution.Css,
            Js = solution.Js,
            Completed = solution.Completed
        };
    }
}