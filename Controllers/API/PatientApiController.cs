﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VitalSignsMonitor.Hubs;
using VitalSignsMonitor.Models;

namespace VitalSignsMonitor.Controllers.API
{
    [Route("api/patient")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<VitalSignsHub> _hub;

        public PatientApiController(ApplicationDbContext context, IHubContext<VitalSignsHub> hub)
        {
            _context = context;
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
        public async Task<IActionResult> PostVitalSigns(int id, [FromBody] Vitals vital)
        {
            if (vital.heartRate < 0 || vital.heartRate > 200)
                return BadRequest("Heart rate must be between 0 and 200.");

            if (vital.systolicBP < 50 || vital.systolicBP > 250)
                return BadRequest("Systolic pressure must be within a safe range.");

            if (vital.diastolicBP < 30 || vital.diastolicBP > 150)
                return BadRequest("Diastolic pressure must be within a safe range.");

            if (vital.oxygenSaturation < 50 || vital.oxygenSaturation > 100)
                return BadRequest("Oxygen saturation must be 50–100%.");

            VitalSign vitalSign = new VitalSign
            {
                HeartRate = vital.heartRate,
                SystolicBP = vital.systolicBP,
                DiastolicBP = vital.diastolicBP,
                OxygenSaturation = vital.oxygenSaturation,
                PatientId = id,
                Timestamp = DateTime.UtcNow

            };

            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveVital", vitalSign);

            return Ok(vitalSign);
        }

        [HttpGet("{id}/vitals/history")]
        public async Task<IActionResult> GetVitalsHistory(int id)
        {
            var since = DateTime.UtcNow.AddHours(-24);

            var vitals = await _context.VitalSigns
                .Where(v => v.PatientId == id && v.Timestamp >= since)
                .OrderBy(v => v.Timestamp)
                .ToListAsync();

            return Ok(vitals);
        }

        [HttpGet("{id}/vitals/export")]
        public async Task<IActionResult> ExportVitalsToCsv(int id)
        {
            var vitals = await _context.VitalSigns
                .Where(v => v.PatientId == id)
                .OrderBy(v => v.Timestamp)
                .ToListAsync();

            if (!vitals.Any())
            {
                return NotFound($"No vital signs found for patient ID {id}.");
            }

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Timestamp,HeartRate,SystolicBP,DiastolicBP,OxygenSaturation");

            foreach (var v in vitals)
            {
                csvBuilder.AppendLine($"{v.Timestamp:o},{v.HeartRate},{v.SystolicBP},{v.DiastolicBP},{v.OxygenSaturation}");
            }

            var fileName = $"vitals_patient_{id}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(csvBytes, "text/csv", fileName);
        }


    }
}
