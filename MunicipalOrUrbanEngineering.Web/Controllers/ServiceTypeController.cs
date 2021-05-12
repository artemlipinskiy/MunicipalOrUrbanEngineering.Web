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
    public class ServiceTypeController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly ServiceTypeService serviceTypeService;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;

        public ServiceTypeController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            serviceTypeService = new ServiceTypeService(db);
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
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> Create(CreateServiceTypeDto dto)
        {
            try
            {
                await GetUserInfo();
                var result = await serviceTypeService.Create(dto, employeeId);
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
                var result = await serviceTypeService.Delete(id, employeeId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> List()
        {
            try
            {
                var result = await serviceTypeService.List();
                return Json(result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee, SysAdmin")]
        public async Task<JsonResult> Get(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return Json(null);
                }
                var result = await serviceTypeService.Get(id.Value);
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
