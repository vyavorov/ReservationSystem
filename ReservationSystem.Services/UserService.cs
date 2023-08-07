using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services;

public class UserService : IUserService
{
    private readonly ReservationDbContext context;

    public UserService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task<string> GetFullNameByEmailAsync(string email)
    {
        ApplicationUser? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return string.Empty;
        }
        return $"{user.FirstName} {user.LastName}";
    }
}
