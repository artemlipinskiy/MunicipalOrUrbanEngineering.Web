using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly MUEContext db;
        private readonly HistoryService historyService;

        public HistoryController(MUEContext db)
        {
            this.db = db;
            historyService = new HistoryService(db);
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            ViewData["page"] = page;
            ViewData["pageSize"] = pageSize;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<JsonResult> List(int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await historyService.List(pageSize, page);
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
