namespace ReservationSystem.Web.ViewModels.Location;

public class LocationDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal PricePerDay { get; set; }

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int Capacity { get; set; }

    public ReviewFormViewModel ReviewForm { get; set; } = null!;

    public bool HasUserReviewed { get; set; }
}
