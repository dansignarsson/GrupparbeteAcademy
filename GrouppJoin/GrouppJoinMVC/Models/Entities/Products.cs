using System;
using System.Collections.Generic;

namespace GroupJoinMVC.Models.Entities
{
    public partial class Products
    {
        public Products()
        {
            Products2Stores = new HashSet<Products2Stores>();
            Rating = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Brand { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<Products2Stores> Products2Stores { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
