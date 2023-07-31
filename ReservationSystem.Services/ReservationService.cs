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
        if (model.PromoCode != null && promoCode == null)
        {
            throw new ArgumentException("Promocode does not exist");
        }
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
            Discount = promoCode?.Discount == null ? 0 : promoCode!.Discount,
            PromoCodeId = promoCode?.Id,
        };
        if (!AreDatesValid(reservation))
        {
            throw new ArgumentException("'To' date must be greater than 'From' date");
        }
        //TODO: FIX TOTAL PRICE TO REFLECT DISCOUNT
        int reservationDays = GetReservationDays(reservation);
        decimal discountToApply = reservation.Discount == 0 ? 1 : (decimal)reservation.Discount / 100;
        reservation.TotalPrice = ((decimal)model.CustomersCount * chosenLocation.PricePerDay * reservationDays) * discountToApply;


        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();

        AddEquipmentsToReservation(model, reservation, context, chosenLocation);

        await context.SaveChangesAsync();
    }

    public void AddEquipmentsToReservation(ReservationFormViewModel model, Reservation reservation, ReservationDbContext context, Location location)
    {
        foreach (var eachEquipmet in model.Equipments)
        {
            if (eachEquipmet.Quantity > 0)
            {
                if (eachEquipmet.Quantity <= model.CustomersCount)
                {
                    EquipmentReservations equipmentReservations = new EquipmentReservations()
                    {
                        EquipmentId = eachEquipmet.Id,
                        ReservationId = reservation.Id,
                        Quantity = eachEquipmet.Quantity,
                    };
                    context.EquipmentsReservations.Add(equipmentReservations);
                }
                else
                {
                    throw new ArgumentException("Please share valid equipment counts");
                }
            }
        }
    }

    public int GetReservationDays(Reservation reservation)
    {
        return (reservation.To - reservation.From).Days + 1;
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

    public async Task<decimal> GetPricePerDayByLocation(int locationId)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
        return location.PricePerDay;
    }

    public async Task<List<ReservationFormViewModel>> GetAllReservationsForUserASync(string userId)
    {
        //TODO: implement isactive
        List<ReservationFormViewModel> reservations = await context.Reservations
                .Include(r => r.PromoCode)
                .Where(r => r.UserId.ToString() == userId)
                .Select(r => new ReservationFormViewModel()
                {
                    Location = r.Location,
                    UserId = r.UserId,
                    AdditionalInformation = r.AdditionalInformation,
                    CustomersCount = r.CustomersCount,
                    From = r.From,
                    To = r.To,
                    CreatedOn = r.CreatedOn,
                    PhoneNumber = r.PhoneNumber,
                    PricePerDay = r.Location.PricePerDay,
                    LocationId = r.LocationId,
                    PromoCode = r.PromoCode.Name,
                    TotalPrice = r.TotalPrice,
                    Discount = r.Discount,
                    Equipments = r.EquipmentNeeded.Select(en => new EquipmentViewModel()
                    {
                        Id = en.EquipmentId,
                        Name = en.Equipment.Name,
                        Quantity = en.Quantity
                    }).ToList()
                })
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();

        return reservations;
    }
}
