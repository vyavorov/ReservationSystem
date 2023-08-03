using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services;

public class PromoCodeService : IPromoCodeService
{
    private readonly ReservationDbContext context;

    public PromoCodeService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task CreatePromoCodeAsync(PromoCode promoCode)
    {
        if (promoCode.Discount <= 0 || promoCode.Discount > 100)
        {
            throw new ArgumentException("Please enter valid discount");
        }
        if (promoCode.Name == null)
        {
            throw new ArgumentException("Please enter valid name");
        }
        await this.context.PromoCodes.AddAsync(promoCode);
        await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id, PromoCode promoCode)
    {
        PromoCode? promoCodeToDelete = await context.PromoCodes
                .Where(pc => pc.IsActive)
                .FirstOrDefaultAsync(pc => pc.Id.ToString() == id);
        if (promoCodeToDelete == null)
        {
            throw new ArgumentException("Promocode does not exist");
        }
        promoCodeToDelete.IsActive = false;
        await context.SaveChangesAsync();
    }

    public async Task<PromoCode> GetPromoCodeByIdAsync(string Id)
    {
        PromoCode? promocode = await context.PromoCodes
                .Where(pc => pc.IsActive)
                .FirstOrDefaultAsync(pc => pc.Id.ToString() == Id);

        if (promocode == null)
        {
            throw new ArgumentException("Promocode does not exist");
        }
        return promocode;
    }

    public async Task<List<PromoCode>> GetPromoCodesAsync()
    {
        List<PromoCode> promoCodes = await context.PromoCodes.Where(pc => pc.IsActive).ToListAsync();

        return promoCodes;
    }
}
