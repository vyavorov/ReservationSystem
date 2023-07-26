using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Services;

public class ReservationService : IReservationService
{
    private readonly ReservationDbContext context;

    public ReservationService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task CreateReservationAsync(ReservationFormViewModel model)
    {
        //TODO: IMPLEMENT ISACTIVE FOR PROMOCODE
        PromoCode? promoCode = await context.PromoCodes.FirstOrDefaultAsync(pc => pc.Name == model.PromoCode);
        Location? chosenLocation = await context.Locations.Where(l => l.IsActive).FirstOrDefaultAsync(l => l.Id == model.LocationId);
        if (chosenLocation == null)
        {
            throw new ArgumentException("Chosen location cannot be found. Please try again");
        }
        if (model.CustomersCount == null || model.CustomersCount <= 0 || model.CustomersCount > chosenLocation.Capacity)
        {
            throw new ArgumentException("Please share valid desks count needed");
        }
        Reservation reservation = new Reservation()
        {
            From = model.From,
            To = model.To,
            AdditionalInformation = model.AdditionalInformation,
            CustomersCount = (int)model.CustomersCount!,
            LocationId = model.LocationId,
            PhoneNumber = model.PhoneNumber,
            UserId = model.UserId,
            Discount = promoCode?.Discount == null ? 0 : promoCode!.Discount
        };
        if (!AreDatesValid(reservation))
        {
            throw new ArgumentException("To date must be greater than From date");
        }
        int reservationDays = GetReservationDays(reservation);
        reservation.TotalPrice = (decimal)((model.CustomersCount * chosenLocation.PricePerDay * reservationDays) - reservation.Discount);

        AddEquipmentsToReservation(model, reservation, context);

        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
    }

    public void AddEquipmentsToReservation(ReservationFormViewModel model, Reservation reservation, ReservationDbContext context)
    {
        foreach (var eachEquipmet in model.Equipments)
        {
            if (eachEquipmet.Quantity > 0)
            {
                EquipmentReservations equipmentReservations = new EquipmentReservations()
                {
                    EquipmentId = eachEquipmet.Id,
                    ReservationId = reservation.Id
                };
                reservation.EquipmentNeeded.Add(equipmentReservations);
            }
        }

    }

    public int GetReservationDays(Reservation reservation)
    {
        return (reservation.To - reservation.From).Days;
    }

    public bool AreDatesValid(Reservation reservation)
    {
        return reservation.To >= reservation.From;
    }

    public async Task<List<EquipmentViewModel>> GetAllEquipmentsAsync()
    {
        List<EquipmentViewModel> allEquipments =
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
