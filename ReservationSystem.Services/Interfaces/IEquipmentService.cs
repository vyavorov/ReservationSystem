using ReservationSystem.Data.Models;

namespace ReservationSystem.Services.Interfaces;

public interface IEquipmentService
{
    public Task<List<Equipment>> GetAllEquipmentsAsync();

    public Task CreateEquipmentAsync(Equipment equipment);

    public Task<Equipment> GetEquipmentByIdAsync(int id);

    public Task DeleteEquipmentAsync(int id, Equipment equipment);
}
