namespace borntocode_backend.Dto;

public class TaskDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string FullDescription { get; set; } = null!;

    public string CodeExample { get; set; } = null!;

    public TaskHtmlStructDto HtmlStruct { get; set; } = null!;
}