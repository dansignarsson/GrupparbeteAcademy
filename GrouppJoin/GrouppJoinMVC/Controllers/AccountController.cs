using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GroupJoinMVC.Models;
using GroupJoinMVC.Models.Data;
using GroupJoinMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GroupJoinMVC.Controllers
{
  
    public class AccountController : Controller
    {
        AccountService service;
        public AccountController(AccountService service)
        {
            this.service = service;
        }


        //Inloggningsida MVC
        [HttpGet]
        [Route("")]
        [Route("/Login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new AccountLoginVM { ReturnUrl = returnUrl });
        }

        //Inlogg via Cordova
        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]AccountLoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return Json(new { modelState = false});

            // Check if credentials is valid (and set auth cookie)
            var result = await service.TryLoginAsync(viewModel);
            if (!result.Succeeded)
            {
                // Show error
                ModelState.AddModelError(nameof(AccountLoginVM.Username), "Login failed");
                return Json(new { success = false, loginResponseText = "Email eller lösenord stämmer inte" });

            }

            var x = service.GetUserIdAndFNameFromDB(viewModel);
            var userID = x[0].Id;
            var firstName = x[0].FirstName;

            // Redirect user
            if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                return Json(new { success = true, responseText = "Inloggning Lyckades", userId = userID, firstName = firstName/*, JsonRequestBehavior.AllowGet*/});

            else
                return Redirect(viewModel.ReturnUrl);
        }


        //Inlogg via MVC
        //[HttpPost]
        //[Route("/Login")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(AccountLoginVM viewModel)
        //{
        //    if (!ModelState.IsValid)
        //        return View(viewModel);

        //    // Check if credentials is valid (and set auth cookie)
        //    var result = await service.TryLoginAsync(viewModel);
        //    if (!result.Succeeded)
        //    {
        //        // Show error
        //        ModelState.AddModelError(nameof(AccountLoginVM.Username), "Login failed");
        //        return View(viewModel);
        //    }

        //    // Redirect user
        //    if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
        //        return RedirectToAction("Search", "Home");
        //    else
        //        return Redirect(viewModel.ReturnUrl);
        //}


        [HttpGet]
        [Route("/Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]AccountRegisterVM viewModel)
        {
            if (!ModelState.IsValid)
                return Json(new { ModelState = false });

            // Try to register user
            var result = await service.TryRegisterAsync(viewModel);
            if (!result.Succeeded)
            {
                var x = result.Errors.First().Description;
                // Show error
                ModelState.AddModelError(string.Empty, result.Errors.First().Description);
                return Json(new { success = false, responseText = "Det gick inte att skapa användaren.", test = x}/*, JsonRequestBehavior.AllowGet*/);
            }
            return Json(new { success = true, responseText = "Användaren tillagd i databasen" }/*, JsonRequestBehavior.AllowGet*/);

        }


        
        [HttpPost]
        [Route("/checkLogin")]        
        public bool CheckLogin([FromBody]string userId)
        {
            return service.CheckAuthToken(userId);
        }

        [TokenFilter]
        [HttpPost]
        [Route("~/email")]
        public IActionResult ReturnEmail([FromBody]UserId data)
        {
            return Json(service.GetUserEmail(data));
        }


        //MVC Logout
        //[HttpPost]
        //[Route("/Logout")]
        //public async Task<IActionResult> Logout()
        //{

        //    await service.TryLogoutAsync();
        //    return RedirectToAction(nameof(Register));
        //}
    }
}