namespace BookShop.Data;

public class Book : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = new();

    public string? ImageBase64 { get; set; } = string.Empty;
}