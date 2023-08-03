using ReservationSystem.Data.Models;

namespace ReservationSystem.Services.Interfaces;

public interface IPromoCodeService
{
    public Task<List<PromoCode>> GetPromoCodesAsync();

    public Task CreatePromoCodeAsync(PromoCode promoCode);

    public Task<PromoCode> GetPromoCodeByIdAsync(string id);

    public Task DeleteAsync(string id, PromoCode promoCode);
}
