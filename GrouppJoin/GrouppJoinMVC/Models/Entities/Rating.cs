using System;
using System.Collections.Generic;

namespace GroupJoinMVC.Models.Entities
{
    public partial class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public double? Rating1 { get; set; }
        public string Comment { get; set; }

        public virtual Products Product { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
