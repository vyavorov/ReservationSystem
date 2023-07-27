using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Web.ViewModels.Reservation;

namespace ReservationSystem.Services.Interfaces;

public interface IReservationService
{
    public Task<List<EquipmentViewModel>> GetAllEquipmentsAsync();
    public Task CreateReservationAsync(ReservationFormViewModel model);

    public bool AreDatesValid(Reservation reservation);

    public int GetReservationDays(Reservation reservation);

    public void AddEquipmentsToReservation(ReservationFormViewModel model, Reservation reservation, ReservationDbContext context, Location location);

    public Task<decimal> GetPricePerDayByLocation(int locationId);
}

