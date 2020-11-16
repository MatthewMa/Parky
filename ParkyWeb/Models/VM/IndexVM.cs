using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.VM
{
    public class IndexVM
    {
        public IEnumerable<Trail> Trails { get; set; }
        public IEnumerable<NationalPark> NationalParks { get; set; }
    }
}
