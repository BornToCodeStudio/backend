namespace borntocode_backend.Dto;

public class SolutionDto
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string CreatedAt { get; set; } = null!;

    public string Html { get; set; } = null!;

    public string Css { get; set; } = null!;

    public string Js { get; set; } = null!;
    
    public bool Completed { get; set; }

    public string Title { get; set; } = null!;
}