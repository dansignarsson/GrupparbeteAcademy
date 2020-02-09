using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class RatingAddVM
    {

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public double? Rating { get; set; }
        public string Comment { get; set; }

       
        public SelectListItem[] ListOfProducts { get; set; }

        //[Range (1,3)]
        public int SelectedProductValue { get; set; }

    }
}
