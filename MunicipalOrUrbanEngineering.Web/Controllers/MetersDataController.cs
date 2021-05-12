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
    public class MetersDataController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;
        private readonly MetersDataService metersDataService;
        private readonly PaymentPeriodService paymentPeriodService;
        private readonly TariffSerivce tariffSerivce;
        private readonly BuildingService buildingService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;
        public MetersDataController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            ownerService = new OwnerService(db);
            employeeService = new EmployeeService(db);
            metersDataService = new MetersDataService(db);
            paymentPeriodService = new PaymentPeriodService(db);
            tariffSerivce = new TariffSerivce(db);
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MyMetersData(Guid flatId)
        {
            var flat = await buildingService.GetFlat(flatId);
            ViewData["flatId"] = flatId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> Create(CreateMetersDataDto dto)
        {
            try
            {
                await GetUserInfo();
                var result = await metersDataService.Create(dto, ownerId, employeeId);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> GetTariffsForTakingMeterReadings(Guid flatId, Guid periodId)
        {
            try
            {
                var period = await paymentPeriodService.Get(periodId);
                if (period == null)
                {
                    return Json(null);
                }
                if (period.StatusId == Constants.PaymentPeriodStatus.CollectingReadings)
                {
                  var tariffs = await tariffSerivce.ListTariffWithCounter(flatId, period.StartDate, period.EndDate);
                  var actualtariffs = new List<ViewTariffDto>();
                  foreach (var tariff in tariffs)
                  {
                     var metersdata = await metersDataService.Get(period.Id, tariff.Id);
                     if (metersdata == null)
                     {
                         actualtariffs.Add(tariff);
                     }
                  }
                  return Json(actualtariffs);
                }

                return Json(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> GetMetersData(Guid flatId, Guid periodId)
        {
            try
            {
                var period = await paymentPeriodService.Get(periodId);
                if (period == null)
                {
                    return Json(null);
                }

                var metersdata = await metersDataService.List(flatId, period.Id);

                return Json(metersdata);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
