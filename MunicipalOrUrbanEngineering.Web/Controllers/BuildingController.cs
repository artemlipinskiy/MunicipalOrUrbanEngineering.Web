using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.DataTransferObjects.Address;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class BuildingController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly BuildingService buildingService;
        private readonly EmployeeService employeeService;
        private readonly OwnerService ownerService;

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
        public BuildingController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            buildingService = new BuildingService(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult MyFlats()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> GetMyFlats()
        {
            await GetUserInfo();
            var result = await buildingService.GetFlatsByOwner(ownerId.Value);
            return Json(result);
        }


        #region Create
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> CreateStreet(CreateStreetDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Create(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Create(dto, null);
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
        public async Task<JsonResult> CreateBuilding(CreateBuildingDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Create(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Create(dto, null);
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
        public async Task<JsonResult> CreateFlat(CreateFlatDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Create(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Create(dto, null);
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> DeleteStreet(Guid id)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.DeleteStreet(id, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.DeleteStreet(id, null);
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
        public async Task<JsonResult> DeleteBuilding(Guid id)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.DeleteBuilding(id, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.DeleteBuilding(id, null);
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
        public async Task<JsonResult> DeleteFlat(Guid id)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.DeleteFlat(id, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.DeleteFlat(id, null);
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Update
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> UpdateStreet(UpdateStreetDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Update(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Update(dto, null);
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
        public async Task<JsonResult> UpdateBuilding(UpdateBuildingDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Update(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Update(dto, null);
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
        public async Task<JsonResult> UpdateFlat(UpdateFlatDto dto)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.Update(dto, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.Update(dto, null);
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
        
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> AssignFlatToOwner(Guid ownerId, Guid flatId)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.AssignFlatToOwner(flatId, ownerId, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.AssignFlatToOwner(flatId, ownerId, null);
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
        public async Task<JsonResult> RemoveAssignFlatToOwner(Guid ownerId, Guid flatId)
        {
            try
            {
                await GetUserInfo();
                if (user.RoleId == Constants.Role.EmployeeRoleId)
                {
                    var employee = await employeeService.GetEmployeeInfo(user.Id);
                    var result = await buildingService.RemoveAssignFlatToOwner(flatId, ownerId, employee.Id);
                    return Json(result);
                }
                else
                {
                    var result = await buildingService.RemoveAssignFlatToOwner(flatId, ownerId, null);
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region List
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListStreet()
        {
            try
            {
                var result = await buildingService.ListStreet();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListBuilding(Guid streetId)
        {
            try
            {
                var result = await buildingService.ListBuilding(streetId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListFlat(Guid buildingId)
        {
            try
            {
                var result = await buildingService.ListFlat(buildingId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListAllBuilding()
        {
            try
            {
                var result = await buildingService.ListBuilding();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListAllFlat()
        {
            try
            {
                var result = await buildingService.ListFlat();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> GetStreet(Guid streetId)
        {
            try
            {
                var result = await buildingService.GetStreet(streetId);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> GetBuilding(Guid buildingId)
        {
            try
            {
                var result = await buildingService.GetBuilding(buildingId);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> GetFlat(Guid flatId)
        {
            try
            {
                var result = await buildingService.GetFlat(flatId);
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
