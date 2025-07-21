using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Controllers.API
{
    [Route("api/patient")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientController> _logger;

        public PatientApiController(ILogger<PatientController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /api/patient
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}/vitals")]
        public async Task<IActionResult> GetVitalsByPatientId(int id)
        {
            var vitals = await _context.VitalSigns
                .Where(v => v.PatientId == id)
                .OrderByDescending(v => v.Timestamp)
                .ToListAsync();

            if (!vitals.Any())
            {
                return NotFound(new { message = $"No vital signs found for patient ID {id}." });
            }

            return Ok(vitals);
        }

        // POST: /api/patient/{id}/vitals
        [HttpPost("{id}/vitals")]
        public async Task<IActionResult> PostVitalSigns(int id, [FromBody] VitalSign vitalSign)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound(new { message = $"Patient with ID {id} not found." });
            }

   
            vitalSign.PatientId = id;
            vitalSign.Timestamp = DateTime.UtcNow;

        
            if (vitalSign.HeartRate <= 0 || vitalSign.OxygenSaturation <= 0)
            {
                return BadRequest(new { message = "Invalid vital sign values." });
            }

            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVitalsByPatientId), new { id = id }, vitalSign);
        }
    }

}
