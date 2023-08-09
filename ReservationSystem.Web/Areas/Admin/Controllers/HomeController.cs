using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly ILocationService locationService;
        private readonly IMemoryCache memoryCache;

        public HomeController(ILocationService locationService, IMemoryCache memoryCache)
        {
            this.locationService = locationService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<IndexViewModel> locations = this.memoryCache.Get<IEnumerable<IndexViewModel>>(LocationsCacheKey);
            if (locations == null)
            {
                locations = await locationService.GetAllLocationsAsync();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(LocationsCacheDurationMinutes));

                this.memoryCache.Set(LocationsCacheKey, locations,cacheOptions);
            }
            return View(locations);
        }
    }
}
