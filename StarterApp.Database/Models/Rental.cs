using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

[Table("rentals")]
public class Rental
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int BorrowerId { get; set; }
    public User? Borrower { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Requested";
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
