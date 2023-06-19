namespace borntocode_backend.Dto;

public class UserDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;
}