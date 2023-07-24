using ReservationSystem.Web.ViewModels.Reservation;

namespace ReservationSystem.Services.Interfaces;

public interface IReservationService
{
    public Task<ICollection<EquipmentViewModel>> GetAllEquipmentsAsync();
}

