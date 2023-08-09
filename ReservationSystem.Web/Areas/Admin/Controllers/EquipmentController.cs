using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers;

public class EquipmentController : BaseAdminController
{
    private readonly IEquipmentService service;

    public EquipmentController(IEquipmentService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        List<Equipment> equipments = await service.GetAllEquipmentsAsync();
        return View(equipments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Equipment equipment)
    {
        await service.CreateEquipmentAsync(equipment);

        return RedirectToAction("All", "Equipment", new { Area = AdminAreaName });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Equipment equipment = await service.GetEquipmentByIdAsync(id);
        if (equipment == null)
        {
            return RedirectToAction("All", "Equipment");
        }
        return View(equipment);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, Equipment equipment)
    {
        await service.DeleteEquipmentAsync(id, equipment);

        return RedirectToAction("All", "Equipment", new { Area = AdminAreaName });
    }
}
