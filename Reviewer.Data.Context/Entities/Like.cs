namespace Reviewer.Data.Context.Entities;

public class Like : EntityBase
{
    public User Owner { get; set; } = null!;
}