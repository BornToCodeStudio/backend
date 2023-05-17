using System.Security.Claims;
using borntocode_backend.Database;
using borntocode_backend.Database.Models;
using borntocode_backend.Dto;
using borntocode_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace borntocode_backend.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private TokenService TokenService { get; }

    public UsersController(TokenService tokenService)
    {
        TokenService = tokenService;
    }
    
    [HttpPost("signIn")]
    public async Task<ActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        await using var context = new ApplicationContext();

        var user = context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Name == signInDto.Name);
        if (user == null)
            return BadRequest("Неверный логин или пароль");

        user.LastLoginAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var verifyPassword = PasswordEncryption.CheckPassword(user.Password, signInDto.Password);
        if (!verifyPassword)
            return BadRequest("Неверный логин или пароль");

        var token = TokenService.CreateToken(user, user.Role);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddMinutes(30)
        };
        
        HttpContext.Response.Cookies.Append("TOKEN", token, cookieOptions);

        return Ok();
    }

    [HttpPost("signUp")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        await using var context = new ApplicationContext();

        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (role == null)
            return BadRequest();

        var userExists = context.Users.Any(u => u.Name == signUpDto.Name);
        if (userExists)
            return BadRequest("Такое имя пользователя уже существует");
        
        var password = PasswordEncryption.Encrypt(signUpDto.Password);
        var user = new User
        {
            Name = signUpDto.Name,
            Password = password,
            Email = "",
            CreatedAt = DateTime.UtcNow,
            RoleId = role.Id
        };

        await context.Users.AddAsync(user);
        
        await context.SaveChangesAsync();

        user.Role = role;
        
        return Ok();
    }
    
    [Authorize]
    [HttpPost("verifySignIn")]
    public ActionResult VerifySignIn()
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("selfProfile")]
    public async Task<ActionResult<int>> GetSelfProfile()
    {
        var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(name))
            return NotFound();

        await using var context = new ApplicationContext();

        var user = context.Users.FirstOrDefault(u => u.Name == name);
        if (user == null)
            return NotFound();

        return user.Id;
    }
    
    [HttpGet("getProfile/{id:int}")]
    public async Task<ActionResult<UserDto>> GetProfile(int id)
    {
        await using var context = new ApplicationContext();

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return NotFound();

        return new UserDto
        {
            Name = user.Name
        };
    }
    
    [Authorize]
    [HttpPut("sendPhoto")]
    public async Task<ActionResult> GetFile([FromForm] IFormFile formFile)
    {
        var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(name))
            return NotFound();

        await using var context = new ApplicationContext();

        var user = await context.Users.FirstOrDefaultAsync(p => p.Name == name);
        if (user == null)
            return BadRequest();

        using var memoryStream = new MemoryStream();
            
        await formFile.CopyToAsync(memoryStream);

        var fileBytes = memoryStream.ToArray();

        user.Avatar = fileBytes;

        await context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("getPhoto/{name}")]
    public async Task<ActionResult<string>> GetAvatar(string name)
    {
        await using var context = new ApplicationContext();

        var user = await context.Users.FirstOrDefaultAsync(p => p.Name == name);
        if (user == null)
            return NotFound();

        if (user.Avatar == null)
            return NotFound();

        var base64 = Convert.ToBase64String(user.Avatar);
        var bytes = Convert.FromBase64String(base64);

        var json = JsonConvert.SerializeObject(new
        {
            avatar = bytes
        });
            
        return json;
    }
}