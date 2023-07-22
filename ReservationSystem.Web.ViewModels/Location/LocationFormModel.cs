using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.Location;

namespace ReservationSystem.Web.ViewModels.Location;

public class LocationFormModel
{

    [Required]
    [StringLength(LocationNameMaxLength,MinimumLength = LocationNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(LocationAddressMaxLength,MinimumLength = LocationAddressMinLength)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(LocationDescriptionMaxLength,MinimumLength = LocationDescriptionMinLength)]
    public string Description { get; set; } = null!;

    [Required]
    public int Capacity { get; set; }

    [Required]
    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; } = null!;

    [Required]
    [Range(typeof(decimal),LocationPricePerDayMinValue,LocationPricePerDayMaxValue)]
    [Display(Name = "Price per day")]
    public decimal PricePerDay { get; set; }

}
