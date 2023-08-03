using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services;

public class EquipmentService : IEquipmentService
{
    private readonly ReservationDbContext context;

    public EquipmentService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task CreateEquipmentAsync(Equipment equipment)
    {
        await context.Equipments.AddAsync(equipment);
        await context.SaveChangesAsync();
    }

    public async Task<Equipment> GetEquipmentByIdAsync(int id)
    {
        Equipment? equipment = await context.Equipments
                .Where(e => e.IsActive)
                .FirstOrDefaultAsync(e => e.Id == id);
        if (equipment != null)
        {
            return equipment;
        }
        return null;

    }

    public async Task<List<Equipment>> GetAllEquipmentsAsync()
    {
        List<Equipment> equipments = await context.Equipments
                .Where(e => e.IsActive)
                .ToListAsync();

        return equipments;
    }

    public async Task DeleteEquipmentAsync(int id, Equipment equipment)
    {
        Equipment? equipmentToRemove = await context.Equipments
                .Where(e => e.IsActive)
                .FirstOrDefaultAsync(e => e.Id == id);

        if (equipmentToRemove != null)
        {
            equipmentToRemove.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
