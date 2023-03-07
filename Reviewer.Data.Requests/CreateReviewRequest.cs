using System.ComponentModel.DataAnnotations;

namespace Reviewer.Data.Requests;

public record CreateReviewRequest
{
    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;
    
    [Range(1, 5)] public int Rating { get; set; }
    
    public string Content { get; set; } = null!;
    
    public string? Dignities { get; set; }
    
    public string? Disadvantages { get; set; }
}