namespace borntocode_backend.Database.Models;

public class Solution
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Html { get; set; }

    public string? Css { get; set; }

    public string? Js { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<SolutionComment> SolutionComments { get; set; } = new List<SolutionComment>();

    public virtual ICollection<SolutionLike> SolutionLikes { get; set; } = new List<SolutionLike>();

    public virtual CodeTask Task { get; set; } = null!;
}
