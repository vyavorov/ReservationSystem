using ReservationSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Web.ViewModels.Reservation;

public class ReservationFormViewModel
{
    public ReservationFormViewModel()
    {
        this.Equipments = new HashSet<EquipmentViewModel>();
    }

    [DataType(DataType.Date)]
    [Required]
    public DateTime From { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime To { get; set; }

    [Required]
    public int CustomersCount { get; set; }

    [Display(Name ="Additional information")]
    public string? AdditionalInformation { get; set; }

    [Display(Name ="Promo code")]
    public string? PromoCode { get; set; }

    public ICollection<EquipmentViewModel> Equipments { get; set; }
}
