namespace borntocode_backend.Database.Models;

public class SolutionComment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SolutionId { get; set; }

    public string? CommentText { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Solution Solution { get; set; } = null!;
}
