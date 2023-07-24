using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;

namespace ReservationSystem.Services;

public class ReservationService : IReservationService
{
    private readonly ReservationDbContext context;

    public ReservationService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task<ICollection<EquipmentViewModel>> GetAllEquipmentsAsync()
    {
        ICollection<EquipmentViewModel> allEquipments =
                await context
                        .Equipments
                        .Select(c => new EquipmentViewModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToListAsync();
        return allEquipments;
    }
}
