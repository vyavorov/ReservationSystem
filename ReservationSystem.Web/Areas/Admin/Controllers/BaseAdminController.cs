﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers;

[Area(AdminAreaName)]
[Authorize(Roles = AdminRoleName)]
public class BaseAdminController : Controller
{

}
