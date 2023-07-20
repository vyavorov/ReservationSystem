using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services;
using ReservationSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount =
            builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
        options.Password.RequireDigit =
            builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
        options.Password.RequireNonAlphanumeric =
            builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
        options.Password.RequiredLength =
            builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
        options.Password.RequireUppercase =
            builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
        options.Password.RequireLowercase =
            builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
    })
    .AddEntityFrameworkStores<ReservationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILocationService, LocationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
