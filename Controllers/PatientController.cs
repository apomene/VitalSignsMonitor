using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Models;
using Microsoft.AspNetCore.Authorization;


namespace VitalSignsMonitor.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PostVitals()
        {
            return View();
        }


        public async Task<IActionResult> VitalSigns(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var vitals = await _context.VitalSigns
                .Where(v => v.PatientId == id)
                .OrderByDescending(v => v.Timestamp)
                .ToListAsync();

            ViewBag.Patient = patient;
            return View(vitals);
        }
      

    }
}

