using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly MUEContext db;
        private readonly UserService userService;
        private readonly EmployeeService employeeService;
        private readonly OwnerService ownerService;

        private UserViewDto user;
        private Guid? employeeId;
        private Guid? ownerId;
        public AccountController(MUEContext db)
        {
            this.db = db;
            userService = new UserService(db);
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
        [Authorize(Roles = "SysAdmin")]
        public IActionResult Users(int page = 1, int pageSize = 10)
        {
            ViewData["page"] = page;
            ViewData["pageSize"] = pageSize;
            return View();
        }
        
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<JsonResult> List(int page = 1, int pageSize = 10)
        {
            var result = await userService.List(pageSize, page);
            return Json(result);
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Owner")]
        public IActionResult Index()
        {
            if (employeeId.HasValue)
            {
                return RedirectToAction(nameof(RegisterOwnerPage));
            }

            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult RegisterEmployeePage()
        {
            return View("RegisterEmployee");
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public ActionResult RegisterOwnerPage()
        {
            return View("RegisterOwner");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<JsonResult> Register(AccountRegisterDto dto)
        {
            var result = await userService.Registration(dto, null, null);
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<JsonResult> RegisterEmployee(AccountRegisterDto dto)
        {
            dto.RoleId = Constants.Role.EmployeeRoleId;
            var result = await userService.Registration(dto, null, null);
            return Json(result);
        }
       
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> RegisterOwner(AccountRegisterDto dto, Guid? flatId = null)
        {
            await GetUserInfo();
            if (user.RoleId == Constants.Role.EmployeeRoleId)
            {
                var employee = await employeeService.GetEmployeeInfo(user.Id);

                var result = await userService.Registration(dto, employee.Id, flatId);
                return Json(result);
            }
            else
            {
                var result = await userService.Registration(dto, null, flatId);
                return Json(result);
            }
        }
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> LoginExist(string login)
        {
            var result = await userService.LoginExist(login);
            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login(AccountLoginDto dto)
        {

            try
            {
                var user = await userService.Get(dto.Login);
                if (user != null)
                {
                    if (await userService.IsCorrectPassword(user, dto.Password))
                    {

                        await Authenticate(user); // аутентификация

                        return Json(true);
                    }
                    else
                    {
                        //TODO
                        return Json(false);
                    }
                }
                else
                {
                    //TODO

                    return Json(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
       
        [HttpPost]
        [Authorize(Roles = "SysAdmin, Employee, Owner")]
        public async Task<JsonResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return Json(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<JsonResult> RoleList()
        {
           var result = Constants.Role.List();
           return Json(result);
        }
        
        private async Task Authenticate(UserViewDto user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
