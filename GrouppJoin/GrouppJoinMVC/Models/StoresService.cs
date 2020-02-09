using GroupJoinMVC.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models
{
    public class StoresService
    {
        public StoresService(GroupJoinDBContext context)
        {
            this.context = context;
        }
        readonly GroupJoinDBContext context;

        public Stores[] GetAllStoresFromDB()
        {
            return context.Stores
                            .OrderBy(p => p.StoreName)
                            .Select(p => new Stores
                            {
                                Id = p.Id,
                                StoreName = p.StoreName
                            })
                            .ToArray();
        }
    }
}
