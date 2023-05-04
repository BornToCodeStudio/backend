namespace borntocode_backend.Database.Models;

public class SolutionLike
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SolutionId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Solution Solution { get; set; } = null!;
}
