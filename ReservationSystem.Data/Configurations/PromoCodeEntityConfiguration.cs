using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Data.Configurations;

public class PromoCodeEntityConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasData(this.GeneratePromoCodes());
    }

    private PromoCode[] GeneratePromoCodes()
    {
        ICollection<PromoCode> promoCodes = new HashSet<PromoCode>();
        PromoCode promoCode;
        promoCode = new PromoCode()
        {
            Name = "internal",
            Discount = 50
        };
        promoCodes.Add(promoCode);
        return promoCodes.ToArray();
    }
}
