using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.DataTransferObjects.Address;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class BulletinBoardController : Controller
    {
        private readonly MUEContext db;
        private readonly BulletinBoardService bulletinBoardService;
        private readonly UserService userService;
        private readonly EmployeeService employeeService;
        private readonly OwnerService ownerService;
        private readonly BuildingService buildingService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;
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
        public BulletinBoardController(MUEContext db)
        {
            this.db = db;
            bulletinBoardService = new BulletinBoardService(db);
            userService = new UserService(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
            buildingService = new BuildingService(db);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> List()
        {
            try
            {
                var result = await bulletinBoardService.List();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> Delete(Guid Id)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await bulletinBoardService.Delete(Id, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await bulletinBoardService.Delete(Id, null);
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> Create(CreateBulletinBoardItemDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    dto.CreatorId = employeeId.Value;
                    var result = await bulletinBoardService.Create(dto, employeeId.Value);
                    return Json(result);
                }
                else
                {
                    var result = await bulletinBoardService.Create(dto, null);
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
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> GetActual()
        {

            await GetUserInfo();
            if (user.RoleId == Constants.Role.OwnerRoleId)
            {
                var owner = await ownerService.GetOwnerInfo(user.Id);
                var flats = await buildingService.GetFlatsByOwner(owner.Id);
                
                var result = await bulletinBoardService.GetActual(flats, owner.Id);
                return Json(result);
            }
            else
            {
                var result = await bulletinBoardService.GetActual(new List<ViewFlatDto>(), null);
                return Json(result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> Get(Guid Id)
        {
            try
            {
                var result = await bulletinBoardService.GetView(Id);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
