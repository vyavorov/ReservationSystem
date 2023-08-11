using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
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

    public async Task CreateReservationAsync(ReservationFormViewModel model)
    {
        PromoCode? promoCode = await context.PromoCodes.Where(pc => pc.IsActive).FirstOrDefaultAsync(pc => pc.Name == model.PromoCode);
        Location? chosenLocation = await context.Locations.Where(l => l.IsActive).FirstOrDefaultAsync(l => l.Id == model.LocationId);

        await this.ValidateReservation(model, promoCode, chosenLocation);
        //reservation creation below
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
        int reservationDays = GetReservationDays(reservation);
        decimal discountToApply = reservation.Discount == 0 ? 1 : 1 - (decimal)reservation.Discount / 100;
        reservation.TotalPrice = ((decimal)model.CustomersCount * chosenLocation.PricePerDay * reservationDays) * discountToApply;

        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();

        AddEquipmentsToReservation(model, reservation, context, chosenLocation);

        await context.SaveChangesAsync();
    }

    public async Task<List<ReservationFormViewModel>> GetAllReservationsForUserASync(string userId)
    {
        List<ReservationFormViewModel> reservations = await context.Reservations
                .Include(r => r.PromoCode)
                .Where(r => r.UserId.ToString() == userId && r.IsActive)
                .Select(r => new ReservationFormViewModel()
                {
                    Id = r.Id,
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
                    Equipments = r.EquipmentNeeded
                        .Where(e => e.Equipment.IsActive)
                        .Select(en => new EquipmentViewModel()
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

    public async Task<ReservationFormViewModel> GetReservationModelByIdAsync(string Id)
    {
        Reservation? reservation = await context.Reservations
            .Where(r => r.IsActive)
            .Include(r => r.EquipmentNeeded)
            .ThenInclude(en => en.Equipment)
            .Include(r => r.Location)
            .Include(r => r.PromoCode)
            .FirstOrDefaultAsync(r => r.Id.ToString() == Id);

        var allEquipments = await this.context.Equipments.Where(e => e.IsActive).ToListAsync();
        if (reservation != null)
        {
            ReservationFormViewModel reservationFormViewModel = new ReservationFormViewModel()
            {
                Id = reservation.Id,
                Location = reservation.Location,
                UserId = reservation.UserId,
                AdditionalInformation = reservation.AdditionalInformation,
                CustomersCount = reservation.CustomersCount,
                From = reservation.From,
                To = reservation.To,
                CreatedOn = reservation.CreatedOn,
                PhoneNumber = reservation.PhoneNumber,
                PricePerDay = reservation.Location.PricePerDay,
                LocationId = reservation.LocationId,
                PromoCode = reservation.PromoCode?.Name,
                TotalPrice = reservation.TotalPrice,
                Discount = reservation.Discount,
                Equipments = allEquipments
                    .Where(e => e.IsActive)
                    .Select(en => new EquipmentViewModel()
                    {
                        Id = en.Id,
                        Name = en.Name,
                        Quantity = reservation.EquipmentNeeded.Any(er => er.EquipmentId == en.Id)
                            ? reservation.EquipmentNeeded.First(er => er.EquipmentId == en.Id).Quantity : 0
                    }).ToList()
            };
            return reservationFormViewModel;
        }
        throw new ArgumentException("Reservation does not exist");
    }

    public async Task EditReservationAsync(string Id, ReservationFormViewModel reservation)
    {
        Reservation? reservationToEdit = await context.Reservations
            .Where(r => r.IsActive)
            .Include(r => r.EquipmentNeeded)
            .ThenInclude(er => er.Equipment)
            .FirstOrDefaultAsync(r => r.Id.ToString() == Id);


        PromoCode? promoCode = await context.PromoCodes.Where(pc => pc.IsActive).FirstOrDefaultAsync(pc => pc.Name == reservation.PromoCode);
        Location? chosenLocation = await context.Locations.Where(l => l.IsActive).FirstOrDefaultAsync(l => l.Id == reservation.LocationId);

        await this.ValidateReservation(reservation, promoCode, chosenLocation);

        if (reservationToEdit != null)
        {
            reservationToEdit.PhoneNumber = reservation.PhoneNumber;
            reservationToEdit.CustomersCount = (int)reservation.CustomersCount!;
            reservationToEdit.AdditionalInformation = reservation.AdditionalInformation;
            reservationToEdit.From = reservation.From;
            reservationToEdit.To = reservation.To;
            if (promoCode != null)
            {
                reservationToEdit.PromoCode = await context.PromoCodes
                        .Where(pc => pc.IsActive)
                        .FirstOrDefaultAsync(pc => pc.Id.ToString() == promoCode.Id.ToString());
            }
            else
            {
                reservationToEdit.PromoCodeId = null;
            }
            reservationToEdit.Discount = promoCode?.Discount == null ? 0 : promoCode!.Discount;

            int reservationDays = GetReservationDays(reservationToEdit);

            decimal discountToApply = reservationToEdit.Discount == 0 ? 1 : 1.00m - (decimal)reservationToEdit.Discount / 100;
            if (reservationToEdit.Discount == 100)
            {
                discountToApply = 0;
            }
            reservationToEdit.TotalPrice = ((decimal)reservationToEdit.CustomersCount * chosenLocation.PricePerDay * reservationDays) * discountToApply;

            AddEquipmentsToReservation(reservation, reservationToEdit, context, reservation.Location);

            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteReservationAsync(string Id, ReservationFormViewModel model)
    {
        Reservation? reservation = await context.Reservations
                .Where(r => r.IsActive)
                .FirstOrDefaultAsync(r => r.Id.ToString() == Id);

        if (reservation == null)
        {
            throw new ArgumentException("This reservation does not exist");
        }
        reservation.IsActive = false;
        await context.SaveChangesAsync();
    }

    public async Task ValidateReservation(ReservationFormViewModel model, PromoCode? promoCode, Location? chosenLocation)
    {

        //check if promocode is valid
        if (model.PromoCode != null && promoCode == null)
        {
            throw new ArgumentException("Promocode does not exist");
        }
        //check if the location is valid
        if (chosenLocation == null)
        {
            throw new ArgumentException("Chosen location cannot be found. Please try again");
        }
        //check if customers count is a valid number
        if (model.CustomersCount == null || model.CustomersCount <= 0 || model.CustomersCount > chosenLocation.Capacity)
        {
            throw new ArgumentException("Please share valid desks count needed");
        }
        //check if the location is available for the dates chosen
        var existingReservations = await context.Reservations
            .Where(r => r.LocationId == model.LocationId
                && r.From.Date <= model.To.Date
                && r.To.Date >= model.From.Date
                && r.IsActive)
            .ToListAsync();

        //check if the location has the capacity
        var totalExistingCustomers = existingReservations.Sum(r => r.CustomersCount);
        if (totalExistingCustomers + model.CustomersCount > chosenLocation.Capacity)
        {
            throw new ArgumentException("The chosen location does not have enough capacity for the requested reservation.");
        }
        //check if dates are valid
        if (!AreDatesValid(model))
        {
            throw new ArgumentException("'To' date must be greater than 'From' date");
        }
    }

    public void AddEquipmentsToReservation(ReservationFormViewModel model, Reservation reservation, ReservationDbContext context, Location location)
    {
        // Get all existing equipment reservations for this reservation
        var existingEquipmentReservations = context.EquipmentsReservations
            .Include(er => er.Equipment)
            .Where(er => er.ReservationId == reservation.Id && er.Equipment.IsActive)
            .ToList();

        foreach (var eachEquipment in model.Equipments)
        {
            var existingEquipmentReservation = existingEquipmentReservations.FirstOrDefault(er => er.EquipmentId == eachEquipment.Id);

            if (eachEquipment.Quantity > 0)
            {
                if (eachEquipment.Quantity <= model.CustomersCount)
                {
                    if (existingEquipmentReservation == null)
                    {
                        // If no existing reservation, add new one
                        EquipmentReservations equipmentReservations = new EquipmentReservations()
                        {
                            EquipmentId = eachEquipment.Id,
                            ReservationId = reservation.Id,
                            Quantity = eachEquipment.Quantity,
                        };
                        context.EquipmentsReservations.Add(equipmentReservations);
                    }
                    else
                    {
                        // If existing reservation, update quantity
                        existingEquipmentReservation.Quantity = eachEquipment.Quantity;
                    }
                }
                else
                {
                    throw new ArgumentException("Please share valid equipment counts");
                }
            }
            else if (existingEquipmentReservation != null)
            {
                // If quantity is 0 and there's an existing reservation, remove it
                context.EquipmentsReservations.Remove(existingEquipmentReservation);
            }
        }
    }

    public int GetReservationDays(Reservation reservation)
    {
        return (reservation.To - reservation.From).Days + 1;
    }

    public bool AreDatesValid(ReservationFormViewModel model)
    {
        return model.To >= model.From;
    }

    public async Task<List<EquipmentViewModel>> GetAllEquipmentsAsync()
    {
        List<EquipmentViewModel> allEquipments =
                    await context
                            .Equipments
                            .Where(e => e.IsActive)
                            .Select(c => new EquipmentViewModel()
                            {
                                Id = c.Id,
                                Name = c.Name
                            }).ToListAsync();
        return allEquipments;
    }

    public async Task<decimal> GetPricePerDayByLocation(int locationId)
    {
        Location? location = await context.Locations
            .Where(l => l.IsActive)
            .FirstOrDefaultAsync(l => l.Id == locationId);
        return location.PricePerDay;
    }

    public async Task<ICollection<AllReservationsViewModel>> GetAllReservationsASync(int? locationId = null)
    {
        IQueryable<Reservation> query = context.Reservations
            .Include(r => r.PromoCode)
            .Where(r => r.IsActive);

        if (locationId.HasValue)
        {
            query = query.Where(r => r.LocationId == locationId.Value);
        }
        ICollection<Location> locations = await context.Locations.Where(l => l.IsActive).ToListAsync();
        ICollection<AllReservationsViewModel> reservations = await query
                .Select(r => new AllReservationsViewModel()
                {
                    Id = r.Id,
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
                    Locations = locations,
                    UserEmail = r.User.Email,
                    Equipments = r.EquipmentNeeded
                        .Where(e => e.Equipment.IsActive)
                        .Select(en => new EquipmentViewModel()
                        {
                            Id = en.EquipmentId,
                            Name = en.Equipment.Name,
                            Quantity = en.Quantity
                        }).ToList()
                })
                .OrderBy(r => r.From)
                .ToListAsync();

        return reservations;
    }

    public async Task<ICollection<Location>> GetAllLocationsAsync()
    {
        ICollection<Location> locations = await context.Locations.Where(l => l.IsActive).ToListAsync();
        return locations;
    }
}

