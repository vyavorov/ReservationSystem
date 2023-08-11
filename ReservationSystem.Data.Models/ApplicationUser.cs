using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.User;

namespace ReservationSystem.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();

        this.UserReservations = new HashSet<Reservation>();
        this.UserReviews = new HashSet<Review>();
    }
    public ICollection<Reservation>? UserReservations { get; set; }

    public ICollection<Review>? UserReviews { get; set; }

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; }
}
