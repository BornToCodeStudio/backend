﻿namespace borntocode_backend.Database.Models;

public class CodeTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CodeExample { get; set; }

    public List<bool>? Languages { get; set; }

    public double CreationDate { get; set; }
    
    public int Likes { get; set; }
    
    public int Views { get; set; }
    
    public string Struct { get; set; } = null!;

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Solution> Solutions { get; set; } = new List<Solution>();

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<TaskLike> TaskLikes { get; set; } = new List<TaskLike>();
}
