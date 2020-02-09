using GroupJoinMVC.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class ProductSearchTotalResultVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Brand { get; set; }
        public string ImgUrl { get; set; }
        public int TotalComments { get; set; }

        public double Rating { get; set; }




    }
}
