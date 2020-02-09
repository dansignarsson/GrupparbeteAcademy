using System;
using System.Collections.Generic;

namespace GroupJoinMVC.Models.Entities
{
    public partial class Stores
    {
        public Stores()
        {
            Products2Stores = new HashSet<Products2Stores>();
        }

        public int Id { get; set; }
        public string StoreName { get; set; }

        public virtual ICollection<Products2Stores> Products2Stores { get; set; }
    }
}
