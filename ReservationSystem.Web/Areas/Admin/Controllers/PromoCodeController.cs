using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers;

public class PromoCodeController : BaseAdminController
{
    private readonly IPromoCodeService service;

    public PromoCodeController(IPromoCodeService service)
    {
        this.service = service;
    }

    public async Task<IActionResult> All()
    {
        List<PromoCode> promoCodes = await service.GetPromoCodesAsync();

        return View(promoCodes);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PromoCode model)
    {
        try
        {
            await service.CreatePromoCodeAsync(model);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        return RedirectToAction("All", "PromoCode", new { Area = AdminAreaName });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string Id)
    {
        PromoCode promoCode = null;
        try
        {
            promoCode = await service.GetPromoCodeByIdAsync(Id);
        }
        catch (Exception)
        {
            return RedirectToAction("All", "PromoCode", new { Area = AdminAreaName });
        }
        return View(promoCode);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string Id, PromoCode promoCode)
    {
        try
        {
            await service.DeleteAsync(Id, promoCode);
        }
        catch (Exception)
        {
            return RedirectToAction("All", "PromoCode", new { Area = AdminAreaName });
        }

        return RedirectToAction("All", "PromoCode", new { Area = AdminAreaName });
    }
}
