using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.Data
{
    public class GetSearchResult
    {
        public string searchText { get; set; }

        public int[] storeIds { get; set; }
    }
}
