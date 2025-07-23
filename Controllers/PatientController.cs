using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly ApplicationDbContext _context;

        public PatientController(ILogger<PatientController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
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

