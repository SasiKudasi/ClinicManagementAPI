using Microsoft.AspNetCore.Mvc;
using ClinicManagementAPI.Application.Contracts;
using ClinicManagementAPI.Application.Services;
using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ICrudService<PatientEntity, PatientDto> _patientService;

        public PatientController(ICrudService<PatientEntity, PatientDto> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(
            [FromQuery] string sortBy = "FullName",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool desc = false)
        {
            var patients = await _patientService.GetList(sortBy, pageNumber, pageSize, desc);
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _patientService.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(PatientDto patientDto)
        {
            var createdPatient = await _patientService.CreateAsync(patientDto);
            return CreatedAtAction(nameof(GetPatient), new { id = createdPatient.Id }, createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto patientDto)
        {
            if (id != patientDto.Id)
            {
                return BadRequest();
            }

            var updatedPatient = await _patientService.UpdateAsync(patientDto);
            if (updatedPatient == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
