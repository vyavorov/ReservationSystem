using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Web.ViewModels.Reservation;

namespace ReservationSystem.Services.Interfaces;

public interface IReservationService
{
    public Task<List<EquipmentViewModel>> GetAllEquipmentsAsync();
    public Task CreateReservationAsync(ReservationFormViewModel model);

    public bool AreDatesValid(ReservationFormViewModel model);

    public int GetReservationDays(Reservation reservation);

    public void AddEquipmentsToReservation(ReservationFormViewModel model, Reservation reservation, ReservationDbContext context, Location location);

    public Task<decimal> GetPricePerDayByLocation(int locationId);

    public Task<List<ReservationFormViewModel>> GetAllReservationsForUserASync(string userId);

    public Task<ReservationFormViewModel> GetReservationModelToByIdAsync(string Id);

    public Task EditReservationAsync(string Id, ReservationFormViewModel reservation);

    public Task ValidateReservation(ReservationFormViewModel model, PromoCode? promoCode, Location? chosenLocation);

    public Task DeleteReservationAsync(string Id, ReservationFormViewModel model);

}

