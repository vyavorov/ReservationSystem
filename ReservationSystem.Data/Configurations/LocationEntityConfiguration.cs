using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Data.Configurations;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasData(this.GenerateLocations());
    }

    private Location[] GenerateLocations()
    {
        ICollection<Location> locations = new HashSet<Location>();

        Location location;
        location = new Location()
        {
            Id = 1,
            Name = "Perla",
            Address = "Perla Beach, Primorsko",
            Capacity = 40,
            PricePerDay = 35,
            ImageUrl = "https://bashhub.bg/wp-content/uploads/2021/10/DSCF9982.png"
        };
        locations.Add(location);

        return locations.ToArray();
    }
}
