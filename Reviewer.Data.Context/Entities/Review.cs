namespace Reviewer.Data.Context.Entities;

public class Review : EntityBase
{
    public User Owner { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Likes { get; set; }
    
    public int Dislikes { get; set; }
}