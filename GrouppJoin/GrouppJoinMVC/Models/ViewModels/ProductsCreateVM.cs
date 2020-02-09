using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class ProductsCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Brand { get; set; }
        public string ImgUrl { get; set; }

        [Display(Name = "Tempers")]
        public SelectListItem[] TemperItems { get; set; }

        // Array som representerar de valda checkboxarnas IDn 
        public int[] SelectedTemperValues { get; set; }



        public ProductsCreateStoresVM[] Stores { get; set; }
    }

    public class ProductsCreateStoresVM
    {
        public string Store { get; set; }
   
    }
}
