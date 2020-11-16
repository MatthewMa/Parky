using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.VM
{
    public class TrailsVM
    {
        public List<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }

    }
}
