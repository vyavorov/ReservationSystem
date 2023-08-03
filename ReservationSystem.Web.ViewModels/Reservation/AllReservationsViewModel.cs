using ReservationSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Web.ViewModels.Reservation;

public class AllReservationsViewModel
{
    public AllReservationsViewModel()
    {
        this.Equipments = new List<EquipmentViewModel>();
        this.Locations = new HashSet<Data.Models.Location>();
    }

    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime From { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime To { get; set; }

    public DateTime CreatedOn { get; set; }

    [Required(ErrorMessage = "Customer count is required.")]
    public int? CustomersCount { get; set; }

    [Display(Name = "Additional information")]
    public string? AdditionalInformation { get; set; }

    [Display(Name = "Promo code")]
    public string? PromoCode { get; set; }

    public List<EquipmentViewModel> Equipments { get; set; }

    public int LocationId { get; set; }

    public Data.Models.Location Location { get; set; }

    public Guid UserId { get; set; }

    [Required(ErrorMessage = "You must provide a phone number")]
    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
    public string PhoneNumber { get; set; } = null!;

    public decimal PricePerDay { get; set; }

    public decimal TotalPrice { get; set; }

    public int? Discount { get; set; }

    public ICollection<Data.Models.Location> Locations { get; set; }
}
