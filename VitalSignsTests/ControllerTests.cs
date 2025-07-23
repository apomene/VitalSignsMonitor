
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using VitalSignsMonitor.Controllers.API;
using VitalSignsMonitor.Models;
using VitalSignsMonitor.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VitalSignsMonitor.Controllers;

namespace VitalSignsTests
{
    [TestFixture]
    public class PatientApiControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<IHubContext<VitalSignsHub>> _hubMock;
        private PatientApiController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Patients.Add(new Patient { Id = 1, Name = "John Doe" });
            _context.SaveChanges();

            // Mock the IClientProxy
            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);

            _hubMock = new Mock<IHubContext<VitalSignsHub>>();
            _hubMock.Setup(h => h.Clients).Returns(mockClients.Object);

            _controller = new PatientApiController(_context, _hubMock.Object);
        }


        [TearDown]
        public void Teardown()
        {
            if (_context != null)
                _context.Dispose();
        }


        [Test]
        public async Task GetAllPatients_ReturnsOkWithPatients()
        {
            var result = await _controller.GetAllPatients();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var patients = okResult?.Value as List<Patient>;

            Assert.That(patients != null);
            Assert.That(1 == patients.Count);
            Assert.That("John Doe" == patients[0].Name);
        }

        [Test]
        public async Task GetVitalsByPatientId_WhenNoVitals_ReturnsNotFound()
        {
            var result = await _controller.GetVitalsByPatientId(1);
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());

        }

        [Test]
        public async Task GetVitalsByPatientId_WithVitals_ReturnsOk()
        {
            _context.VitalSigns.Add(new VitalSign
            {
                PatientId = 1,
                HeartRate = 80,
                SystolicBP = 120,
                DiastolicBP = 80,
                OxygenSaturation = 97,
                Timestamp = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            var result = await _controller.GetVitalsByPatientId(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var vitals = okResult?.Value as List<VitalSign>;

            Assert.That((vitals != null));
            Assert.That(1 == vitals.Count);
            Assert.That(80 == vitals[0].HeartRate);
        }

        [Test]
        public async Task PostVitalSigns_WithValidVitals_ReturnsOk()
        {
            var dto = new Vitals
            {
                heartRate = 75,
                systolicBP = 120,
                diastolicBP = 80,
                oxygenSaturation = 95
            };

            var result = await _controller.PostVitalSigns(1, dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            var vital = ok?.Value as VitalSign;

            Assert.That(vital != null);
            Assert.That(75 == vital.HeartRate);
            Assert.That(1 == vital.PatientId);
        }

        [Test]
        public async Task PostVitalSigns_InvalidHeartRate_ReturnsBadRequest()
        {
            var dto = new Vitals
            {
                heartRate = -5,
                systolicBP = 120,
                diastolicBP = 80,
                oxygenSaturation = 95
            };

            var result = await _controller.PostVitalSigns(1, dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var bad = result as BadRequestObjectResult;
            Assert.That("Heart rate must be between 0 and 200." == bad?.Value);
        }

        [Test]
        public async Task PostVitalSigns_InvalidSystolicBP_ReturnsBadRequest()
        {
            var dto = new Vitals
            {
                heartRate = 75,
                systolicBP = 300,
                diastolicBP = 80,
                oxygenSaturation = 95
            };

            var result = await _controller.PostVitalSigns(1, dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var bad = result as BadRequestObjectResult;
            Assert.That("Systolic pressure must be within a safe range." == bad?.Value);
        }

        [Test]
        public async Task PostVitalSigns_InvalidDiastolicBP_ReturnsBadRequest()
        {
            var dto = new Vitals
            {
                heartRate = 75,
                systolicBP = 120,
                diastolicBP = 200,
                oxygenSaturation = 95
            };

            var result = await _controller.PostVitalSigns(1, dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var bad = result as BadRequestObjectResult;
            Assert.That("Diastolic pressure must be within a safe range." == bad?.Value);
        }

        [Test]
        public async Task PostVitalSigns_InvalidOxygenSaturation_ReturnsBadRequest()
        {
            var dto = new Vitals
            {
                heartRate = 75,
                systolicBP = 120,
                diastolicBP = 80,
                oxygenSaturation = 30
            };

            var result = await _controller.PostVitalSigns(1, dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var bad = result as BadRequestObjectResult;
            Assert.That("Oxygen saturation must be 50–100%." == bad?.Value);
        }

        [Test]
        public async Task GetVitalsHistory_ReturnsVitalsFromLast24Hours()
        {
            // Arrange
            var patientId = 1;
            var now = DateTime.UtcNow;

            // Add one older than 24h (should NOT be returned)
            _context.VitalSigns.Add(new VitalSign
            {
                PatientId = patientId,
                HeartRate = 70,
                SystolicBP = 110,
                DiastolicBP = 70,
                OxygenSaturation = 98,
                Timestamp = now.AddHours(-25)
            });

            // Add one within last 24h (should be returned)
            _context.VitalSigns.Add(new VitalSign
            {
                PatientId = patientId,
                HeartRate = 80,
                SystolicBP = 120,
                DiastolicBP = 80,
                OxygenSaturation = 95,
                Timestamp = now.AddHours(-1)
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetVitalsHistory(patientId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var vitals = okResult.Value as List<VitalSign>;
            Assert.That(vitals, Is.Not.Null);
            Assert.That(vitals.Count, Is.EqualTo(1));
            Assert.That(vitals[0].HeartRate, Is.EqualTo(80));
        }

        [Test]
        public async Task GetVitalsHistory_WhenNoRecentVitals_ReturnsEmptyList()
        {
            // Arrange
            var patientId = 2;

            _context.Patients.Add(new Patient { Id = patientId, Name = "Jane Doe" });

            _context.VitalSigns.Add(new VitalSign
            {
                PatientId = patientId,
                HeartRate = 72,
                SystolicBP = 115,
                DiastolicBP = 75,
                OxygenSaturation = 96,
                Timestamp = DateTime.UtcNow.AddHours(-30) // older than 24 hours
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetVitalsHistory(patientId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);

            var vitals = ok.Value as List<VitalSign>;
            Assert.That(vitals, Is.Not.Null);
            Assert.That(vitals.Count, Is.EqualTo(0)); // should be empty
        }

        [Test]
        public async Task GetVitalsHistory_WhenPatientHasNoVitals_ReturnsEmptyList()
        {
            // Arrange
            var patientId = 3;

            _context.Patients.Add(new Patient { Id = patientId, Name = "Empty Patient" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetVitalsHistory(patientId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);

            var vitals = ok.Value as List<VitalSign>;
            Assert.That(vitals, Is.Not.Null);
            Assert.That(vitals.Count, Is.EqualTo(0));
        }



    }
}

