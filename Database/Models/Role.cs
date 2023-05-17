namespace borntocode_backend.Database.Models;

public class Role
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;
}