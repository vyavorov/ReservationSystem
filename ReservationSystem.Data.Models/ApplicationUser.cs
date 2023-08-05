using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Data.Models;

public class ApplicationUser:IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.UserReservations = new HashSet<Reservation>();
        this.UserReviews = new HashSet<Review>();
    }
    public ICollection<Reservation>? UserReservations { get; set; }

    public ICollection<Review>? UserReviews { get; set; }
}
