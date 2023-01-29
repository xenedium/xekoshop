using xekoshop.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public List<CartLine> CartLines { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}