using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;

namespace Calenderapp.MVC.Controllers
{
    public class SettingsController(CalenderAppContext context) : Controller
    {
        private readonly CalenderAppContext _context = context;

        // GET: Settings
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Settings.ToListAsync());
        }

        // GET: Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var settings = await _context.Settings.FindAsync(id);
                if (settings == null)
                {
                    return NotFound();
                }
                return View(settings);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // POST: Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            var settings = new Settings();
            try
            {
                if (id != Convert.ToInt32(collection["SettingsId"]))
                {
                    return NotFound();
                }
                settings = new Settings
                {
                    SettingsId = Convert.ToInt32(collection["SettingsId"]),
                    PaginationDocent = Convert.ToInt32(collection["PaginationDocent"]),
                    PaginationLokaal = Convert.ToInt32(collection["PaginationLokaal"]),
                    PaginationModules = Convert.ToInt32(collection["PaginationModules"]),
                    PaginationOpleiding = Convert.ToInt32(collection["PaginationOpleiding"]),
                    PaginationPlanning = Convert.ToInt32(collection["PaginationPlanning"]),
                    PaginationVakantie = Convert.ToInt32(collection["PaginationVakantie"]),
                    ContractDeeltijds = Convert.ToInt32(collection["ContractDeeltijds"]),
                    ContractVoltijds = Convert.ToInt32(collection["ContractVoltijds"]),

                };
                if (ModelState.IsValid)
                {

                        _context.Update(settings);
                        await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Jouw gegevens is niet correct ingevuld";
                    return View(settings);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(settings);
            }
        }
    }
}
