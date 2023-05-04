namespace borntocode_backend.Database.Models;

public class TaskComment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }

    public string? CommentText { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual CodeTask Task { get; set; } = null!;
}
