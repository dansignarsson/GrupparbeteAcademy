using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupJoinMVC.Models;
using GroupJoinMVC.Models.Data;
using GroupJoinMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GroupJoinMVC.Controllers
{
    public class ProductsController : Controller
    {


        ProductService service;
        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("~/create")]
        public IActionResult Create()
        {
            return View(service.GetAllStoresFromDBForPCreateVM());
        }

        //[TokenFilter]
        [HttpPost]
        [Route("~/create")]
        public IActionResult Create(ProductsCreateVM product)
        {
            if (!ModelState.IsValid)
                return View(product);

            service.AddItemToDB(product);
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        [Route("~/searchResult/{id}")]
        public IActionResult GetSearchValue(string id)
        {
            return Json(service.GetProductsCategorizedByInputText(id));
        }

        [TokenFilter]
        [HttpPost]
        [Route("~/searchTotalResult/")]
        public IActionResult GetSearchTotalValue([FromBody]GetSearchResult data)
        {
            return Json( service.GetSearchTotalResult(data));
        }

        [HttpGet]
        [Route("~/Bootstrap/{id}")]
        public IActionResult GetSearchTotalValue_MICKETEST(string id)
        {
            return PartialView("_Bootstrap", service.GetSearchTotalResult(id).ToArray());
        }

        [HttpGet]
        [Route("~/test")]
        public IActionResult Search()
        {
            return View();
        }

        [TokenFilter]
        [HttpGet]
        [Route("~/GetDetailsById/{id}")]
        public IActionResult GetDetailsById(int id)
        {
            return Json(service.GetDetailsByID(id));
        }
    }
}