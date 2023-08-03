using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Web.Controllers;

public class PromoCodeController : Controller
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
        return RedirectToAction("All","PromoCode");
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
            return RedirectToAction("All", "PromoCode");
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
            return RedirectToAction("All", "PromoCode");
        }

        return RedirectToAction("All", "PromoCode");
    }
}
