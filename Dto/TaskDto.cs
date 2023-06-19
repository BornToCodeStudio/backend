namespace borntocode_backend.Dto;

public class TaskDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int AuthorId { get; set; }

    public string ShortDescription { get; set; } = null!;

    public string FullDescription { get; set; } = null!;

    public string CodeExample { get; set; } = null!;

    public List<bool> Languages { get; set; } = null!;

    public double CreationDate { get; set; }
    
    public int Likes { get; set; }
    
    public int Views { get; set; }

    public TaskHtmlStructDto HtmlStruct { get; set; } = null!;
}