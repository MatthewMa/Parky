using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.VM;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _tRepo;
        private readonly INationalParkRepository _nRepo;
        public TrailsController(ITrailRepository tRepo, INationalParkRepository nRepo)
        {
            _tRepo = tRepo;
            _nRepo = nRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail());
        }
        public async Task<IActionResult> GetAlltrails()
        {
            IEnumerable<Trail> trails = await _tRepo.GetAllAsync(SD.TrailAPIPath);
            return Json(new { data = trails });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var obj = new Trail();
            var trailsVM = new TrailsVM();
            trailsVM.NationalParkList = new List<SelectListItem>();
            // Get all national park name list
            var parks = await _nRepo.GetAllAsync(SD.NationalParkAPIPath);
            foreach (var park in parks)
            {
                trailsVM.NationalParkList.Add(new SelectListItem() { 
                    Text = park.Name,
                    Value = park.Id.ToString()
                });
            }
            if (id == null)
            {
                trailsVM.Trail = obj;
                return View(trailsVM);
            }
            obj = await _tRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            trailsVM.Trail = obj;
            return View(trailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Trail trail)
        {
            if(ModelState.IsValid)
            {
                if (trail.Id == 0)
                {
                    await _tRepo.CreateAsync(SD.TrailAPIPath, trail);
                }
                else
                {
                    await _tRepo.UpdateAsync(SD.TrailAPIPath + "/" + trail.Id, trail);
                }               
                return RedirectToAction(nameof(Index));               
            }
            else
            {
                var trailsVM = new TrailsVM();
                trailsVM.NationalParkList = new List<SelectListItem>();
                // Get all national park name list
                var parks = await _nRepo.GetAllAsync(SD.NationalParkAPIPath);
                foreach (var park in parks)
                {
                    trailsVM.NationalParkList.Add(new SelectListItem()
                    {
                        Text = park.Name,
                        Value = park.Id.ToString()
                    });
                }
                trailsVM.Trail = trail;
                return View(trail);
            }           
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _tRepo.DeleteAsync(SD.TrailAPIPath, Id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }

    }
}
