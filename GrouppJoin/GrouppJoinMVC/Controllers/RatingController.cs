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
    public class RatingController : Controller
    {
        
        RatingService ratingService;
        public RatingController(RatingService rService)
        {
            this.ratingService = rService;
        }


        [HttpGet]
        [Route("~/rating/add")]
        public IActionResult Add()
        {

            return View(ratingService.GetAllProductsForDropDownList());
        }

        [HttpPost]
        [Route("~/rating/add")]
        public IActionResult Add(RatingAddVM ra)
        {
            if (!ModelState.IsValid)
                return View(ra);

            ratingService.AddRatingToDB(ra);
            return RedirectToAction(nameof(Add));

        }

        [TokenFilter]
        [HttpPost]
        [Route("~/setrating")]
        public IActionResult SetRating([FromBody]RatingAddVM product)
        {
            if (ratingService.AddUserRatingToDB(product))
                return Json(new { success = true});
            else
                return Json(new { success = false });
        }

        [TokenFilter]
        [HttpPost]
        [Route("~/myPage")]
        public IActionResult MyPage([FromBody]UserId data)
        {
            return Json(ratingService.GetUserRatings(data)); 
        }



        [TokenFilter]
        [HttpPost]
        [Route("~/deleteRating")]
        public IActionResult DeleteRating([FromBody]RatingId data)
        {
            return Json(ratingService.DeleteRatingFromDB(data));
        }

    }
}