using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Data.Models;

public class ApplicationUser:IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.UserReservations = new HashSet<Reservation>();
    }
    public ICollection<Reservation>? UserReservations { get; set; }
}
