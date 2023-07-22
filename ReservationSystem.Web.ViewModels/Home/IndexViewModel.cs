namespace ReservationSystem.Web.ViewModels.Home;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal PricePerDay { get; set; }
}
