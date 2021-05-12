using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class PaymentSheetController : Controller
    {

        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly PaymentSheetService paymentSheetService;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;
        private readonly BuildingService buildingService;
        private readonly ServiceBillService serviceBillService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;
        public PaymentSheetController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            paymentSheetService = new PaymentSheetService(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
            buildingService = new BuildingService(db);
            serviceBillService = new ServiceBillService(db);
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
        
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public IActionResult Index(Guid? flatId = null, Guid? statusId = null, Guid? periodId = null)
        {
            ViewData["flatId"] = flatId;
            ViewData["statusId"] = statusId;
            ViewData["periodId"] = periodId;
            return View();
        }
        
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult MySheets(Guid? flatId)
        {
            if (flatId.HasValue)
            {
                ViewData["flatid"] = flatId;
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> List(Guid? flatId = null, Guid? statusId = null, Guid? periodId = null)
        {
            try
            {
                await GetUserInfo();
                if (flatId.HasValue)
                {
                    var sheets = await paymentSheetService.ListByFlat(flatId.Value);
                    return Json(sheets);
                }

                if (statusId.HasValue && periodId.HasValue)
                {
                    var sheets = await paymentSheetService.ListByStatusAndPeriod(statusId.Value, periodId.Value);
                    return Json(sheets);
                }

                if (periodId.HasValue)
                {
                    var sheets = await paymentSheetService.ListByPeriod(periodId.Value);
                    return Json(sheets);
                }

                if (statusId.HasValue)
                {
                    var sheets = await paymentSheetService.ListByStatus(statusId.Value);
                    return Json(sheets);
                }

                if (ownerId.HasValue)
                {
                    var flats = await buildingService.GetFlatsByOwner(ownerId.Value);
                    var flatIds = flats.Select(x => x.Id).ToList();
                    var sheets = await paymentSheetService.ListByFlats(flatIds);
                    return Json(sheets);

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
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> GetDetails(Guid sheetId)
        {
            try
            {
                await GetUserInfo();
                var bills = await serviceBillService.List(sheetId);

                return Json(bills);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> Pay(Guid paymentsheetId)
        {
            try
            {
               await GetUserInfo();
               var result =  await paymentSheetService.Pay(paymentsheetId, employeeId);
               return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> PartPay(Guid serviceBillId)
        {
            try
            {
                await GetUserInfo();
                var result = await paymentSheetService.PartPay(serviceBillId, employeeId);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
