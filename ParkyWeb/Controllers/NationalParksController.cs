using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {            
            return View(new NationalPark { });
        }
        public async Task<IActionResult> Upsert(int? Id)
        {
            var obj = new NationalPark();
            if (Id == null)
            {
                return View(obj);
            }
            obj = await _npRepo.GetAsync(SD.NationalParkAPIPath, Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        public async Task<IActionResult> GetAllNationalParks()
        { 
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    // File to byte[]
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var objFromDB = await _npRepo.GetAsync(SD.NationalParkAPIPath, obj.Id);
                    if (obj.Picture != null)
                    {
                        obj.Picture = objFromDB.Picture;
                    }                  
                }
                if (obj.Id == 0)
                {
                    // Create
                    await _npRepo.CreateAsync(SD.NationalParkAPIPath, obj);
                }
                else
                {
                    // Update
                    await _npRepo.UpdateAsync(SD.NationalParkAPIPath + "/" + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);           
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _npRepo.DeleteAsync(SD.NationalParkAPIPath, Id);
            if(status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}
