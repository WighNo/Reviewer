namespace Reviewer.Data.Context.Entities;

public class Review : EntityBase
{
    public User Owner { get; set; } = null!;

    public string Title { get; set; } = null!;
    
    public string Content { get; set; } = null!;
    
    public string? Dignities { get; set; }
    
    public string? Disadvantages { get; set; }

    public List<Like>? Likes { get; set; } = new();

    public List<Dislike>? Dislikes { get; set; } = new();
}