namespace Reviewer.Data.Context.Entities;

public class Dislike : EntityBase
{
    public User Owner { get; set; } = null!;
}