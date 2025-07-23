using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Hubs;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Controllers.API
{
    [Route("api/patient")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientController> _logger;
        private readonly IHubContext<VitalSignsHub> _hub;

        public PatientApiController(ILogger<PatientController> logger, ApplicationDbContext context, IHubContext<VitalSignsHub> hub)
        {
            _context = context;
            _logger = logger;
            _hub = hub;
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
            vitalSign.PatientId = id;
            vitalSign.Timestamp = DateTime.UtcNow;

            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveVital", vitalSign);

            return Ok(vitalSign);
        }
    }

}
