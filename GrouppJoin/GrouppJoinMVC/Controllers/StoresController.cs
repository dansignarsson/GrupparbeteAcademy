using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupJoinMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroupJoinMVC.Controllers
{
    public class StoresController : Controller
    {
        StoresService service;
        public StoresController(StoresService service)
        {
            this.service = service;
        }

        [TokenFilter]
        [HttpPost]
        [Route("~/ShowStores")]
        public IActionResult ShowStores()
        {
            return Json(service.GetAllStoresFromDB());
        }
    }
}