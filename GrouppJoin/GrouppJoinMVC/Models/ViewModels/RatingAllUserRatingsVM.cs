using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class RatingAllUserRatingsVM
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public UserRatings[] Ratings { get; set; }
    }

    public class UserRatings
    {
        public int? RatingID { get; set; }
        public int? ProductID { get; set; }

        public string ProductName { get; set; }

        public double? AVGRating { get; set; }
        public string ImgUrl { get; set; }
        public double? UserRating { get; set; }
        public string UserComment { get; set; }
    }
}
