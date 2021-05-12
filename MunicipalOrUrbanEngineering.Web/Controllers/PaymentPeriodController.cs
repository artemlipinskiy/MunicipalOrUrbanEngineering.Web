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
    public class PaymentPeriodController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly PaymentPeriodService paymentPeriodService;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;

        public PaymentPeriodController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            paymentPeriodService = new PaymentPeriodService(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
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

        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> Create(CreatePaymentPeriodDto dto)
        {
            try
            {
                await GetUserInfo();
                var result = await paymentPeriodService.Create(dto, employeeId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                await GetUserInfo();
                var result = await paymentPeriodService.Delete(id, employeeId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> List(Guid? statusId)
        {
            try
            {
                if (statusId.HasValue)
                {
                    var result = await paymentPeriodService.List(statusId.Value);
                    return Json(result);
                }
                else
                {

                    var result = await paymentPeriodService.List();
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> Get(Guid id)
        {
            try
            {
                var result = await paymentPeriodService.Get(id);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> GetCurrent()
        {
            try
            {
                var period = await paymentPeriodService.Get(DateTime.Now.Date);
                return Json(period);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin, Owner")]
        public async Task<JsonResult> GetPrev()
        {
            try
            {
                var period = await paymentPeriodService.Get(DateTime.Now.Date);
                period = await paymentPeriodService.GetPreviousPeriod(period.Id);
                return Json(period);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> SetStatus(Guid periodId, Guid statusId)
        {
            try
            {
                await GetUserInfo();
                bool result;
                if (statusId == Constants.PaymentPeriodStatus.CollectingReadings)
                {
                     result = await paymentPeriodService.StartCollectingReadings(periodId, employeeId);
                }
                else if (statusId == Constants.PaymentPeriodStatus.FormationOfReceipts)
                {
                    result = await paymentPeriodService.StartFormationReceipts(periodId, employeeId);
                }
                else if (statusId == Constants.PaymentPeriodStatus.PaymentOfReceipts)
                {
                    result = await paymentPeriodService.StartPaymentOfReceipts(periodId, employeeId);
                }
                else if(statusId == Constants.PaymentPeriodStatus.Close)
                {
                    result = await paymentPeriodService.ClosePeriod(periodId, employeeId);
                }
                else
                {
                    result = false;
                }
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
