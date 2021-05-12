using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class OwnerController : Controller
    {
        private readonly MUEContext db;
        private readonly OwnerService ownerService;

        public OwnerController(MUEContext db)
        {
            this.db = db;
            ownerService = new OwnerService(db);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, Employee")]
        public async Task<JsonResult> ListWithoutFlat()
        {
            try
            {
                var result = await ownerService.ListWithoutFlat();
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
        public async Task<JsonResult> List()
        {
            try
            {
                var result = await ownerService.List();
                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
