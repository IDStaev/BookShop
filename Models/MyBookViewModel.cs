namespace BookShop.Models;

public class MyBookViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string ImageBase64 { get; set; }
}