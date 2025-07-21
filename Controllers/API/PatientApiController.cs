using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Controllers.API
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public IActionResult GetPatients() => Ok(_context.Patients.ToList());

        [HttpGet("{id}/vitals")]
        public IActionResult GetVitals(int id) =>
            Ok(_context.VitalSigns.Where(v => v.PatientId == id).OrderByDescending(v => v.Timestamp));

        [HttpPost("{id}/vitals")]
        public IActionResult PostVitals(int id, [FromBody] VitalSign vital)
        {
            vital.PatientId = id;
            vital.Timestamp = DateTime.UtcNow;
            _context.VitalSigns.Add(vital);
            _context.SaveChanges();
            return Ok(vital);
        }
    }

}
