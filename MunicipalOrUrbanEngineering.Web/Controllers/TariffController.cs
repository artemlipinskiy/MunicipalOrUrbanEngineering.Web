using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.DataTransferObjects.Payment;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class TariffController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly TariffSerivce tariffSerivce;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;
        private readonly BuildingService buildingService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;

        public TariffController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            tariffSerivce = new TariffSerivce(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
            buildingService = new BuildingService(db);
        }

        private async Task GetUserInfo()
        {
            user = await userService.Get(User.Identity.Name);
            if (user.RoleId == Constants.Role.EmployeeRoleId)
            {
                employeeId = (await employeeService.GetEmployeeInfo(user.Id)).Id;
            }
            if (user.RoleId == Constants.Role.OwnerRoleId)
            {
                ownerId = (await ownerService.GetOwnerInfo(user.Id)).Id;
            }
        }


        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> Create(CreateTariffDto dto)
        {
            await GetUserInfo();
            await tariffSerivce.Create(dto, employeeId);
            return Json(true);
        }
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> Delete(Guid id)
        {
            await GetUserInfo();
            await tariffSerivce.Delete(id, employeeId);
            return Json(true);
        }
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> End(Guid id)
        {
            await GetUserInfo();
            await tariffSerivce.End(id, employeeId);
            return Json(true);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee, Owner")]
        public async Task<JsonResult> Get(Guid id)
        {
            await GetUserInfo();
            var result = await tariffSerivce.Get(id);
            return Json(result);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee, Owner")]
        public async Task<JsonResult> List(Guid FlatId, DateTime? date)
        {

            if (date.HasValue)
            {
                var result = await tariffSerivce.List(FlatId, date.Value);
                return Json(result);
            }
            else
            {
                if (User.IsInRole("Employee") || User.IsInRole("SysAdmin"))
                {
                    var result = await tariffSerivce.List(FlatId);
                    return Json(result);
                }
                else
                {
                    date = DateTime.Now;
                    var result = await tariffSerivce.List(FlatId, date.Value);
                    return Json(result);
                }
               
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid FlatId)
        {
            var flat = await buildingService.GetFlat(FlatId);
            ViewData["flatId"] = FlatId;
            ViewData["flat"] = flat.FullAddress;
            ViewData["date"] = $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyTariffs(Guid FlatId)
        {
            var flat = await buildingService.GetFlat(FlatId);
            ViewData["flatId"] = FlatId;
            ViewData["flat"] = flat.FullAddress;
            ViewData["date"] = $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}";
            return View();
        }
    }
}
