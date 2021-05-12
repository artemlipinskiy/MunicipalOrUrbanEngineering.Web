using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.DataTransferObjects.Request;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly RequestService requestService;
        private readonly OwnerService ownerService;
        private readonly EmployeeService employeeService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;
        public RequestController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
            requestService = new RequestService(db);
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
        }
        private async Task GetUserInfo() {
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

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult MyRequest()
        {
            return View();
        }
        

        [HttpPost]
        [Authorize(Roles = "Owner, Employee, SysAdmin")]
        public async Task<JsonResult> Delete(Guid Id)
        {
            try
            {
                await GetUserInfo();
                var result = await requestService.DeleteRequest(Id, ownerId, employeeId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> Create(CreateRequestDto dto)
        {
            try
            {
                await GetUserInfo();
                dto.RequestorId = (Guid) ownerId;
                var result = await requestService.Create(dto, (Guid)ownerId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<JsonResult> Cancel(Guid requestId)
        {
            try
            {
                await GetUserInfo();
                var result = await requestService.SetStatus(requestId, Constants.RequestStatus.Cancel, ownerId, employeeId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> CreateResponse(CreateResponseDto dto)
        {
            try
            {
                await GetUserInfo();
                dto.ResponserId = employeeId.Value;
                var result = await requestService.Create(dto, employeeId);
                if (result == true)
                {
                    await requestService.SetStatus(dto.ServiceRequestId, Constants.RequestStatus.Completed, ownerId, employeeId);
                }
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ResponseList()
        {
            try
            {
                var result = await requestService.ListResponse();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee, Owner")]
        public async Task<JsonResult> RequestList()
        {
            try
            {
                await GetUserInfo();
                if (ownerId == null)
                {

                    var result = await requestService.ListRequest();
                    return Json(result);
                }
                else
                {
                    var result = await requestService.ListRequest(ownerId.Value);
                    return Json(result);
                }            
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Owner, Employee")]
        public async Task<JsonResult> GetRequest(Guid Id)
        {
            try
            {
                var result = await requestService.GetRequest(Id);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Owner, Employee")]
        public async Task<JsonResult> GetResponse(Guid requestId)
        {
            try
            {
                var result = await requestService.GetResponseForRequest(requestId);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Owner, Employee")]
        public async Task<JsonResult> TypeRequestList()
        {
            try
            {
                var result = await requestService.ListType();
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
