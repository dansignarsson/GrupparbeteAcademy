using GroupJoinMVC.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Brand { get; set; }
        public string ImgUrl { get; set; }
        public double AVGRating { get; set; }
        public ProductDetailsRatingVM[] Ratings { get; set; }
    }

    public class ProductDetailsRatingVM
    {
        public string FirstName { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }
    }
}
