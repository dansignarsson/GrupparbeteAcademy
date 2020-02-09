using System;
using System.Collections.Generic;

namespace GroupJoinMVC.Models.Entities
{
    public partial class Products2Stores
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }

        public virtual Products Product { get; set; }
        public virtual Stores Store { get; set; }
    }
}
