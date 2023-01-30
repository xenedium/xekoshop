using xekoshop.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public List<CartLine> CartLines { get; set; } = null!;
    public int ArticleCount { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
}