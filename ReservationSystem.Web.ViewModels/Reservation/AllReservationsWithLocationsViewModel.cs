namespace ReservationSystem.Web.ViewModels.Reservation;

public class AllReservationsWithLocationsViewModel
{
    public ICollection<AllReservationsViewModel> Reservations { get; set; }
    public ICollection<Data.Models.Location> Locations { get; set; }
}
