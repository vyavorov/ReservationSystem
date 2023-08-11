﻿using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.Location;
namespace ReservationSystem.Data.Models;

public class Location
{
    public Location()
    {
        this.Reservations = new HashSet<Reservation>();
        this.Reviews = new HashSet<Review>();
        this.IsActive = true;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(LocationNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(LocationAddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(LocationDescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    public int Capacity { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public decimal PricePerDay { get; set; }

    [Required]
    public ICollection<Reservation> Reservations { get; set; } = null!;

    public bool IsActive { get; set; }

    [Required]
    public ICollection<Review> Reviews { get; set; }

}
