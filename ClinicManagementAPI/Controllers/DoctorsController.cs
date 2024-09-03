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
    public class DoctorController : ControllerBase
    {
        private readonly ICrudService<DoctorEntity, DoctorDto> _doctorService;

        public DoctorController(ICrudService<DoctorEntity, DoctorDto> doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors(
            [FromQuery] string sortBy = "FullName",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool desc = false)
        {
            var doctors = await _doctorService.GetList(sortBy, pageNumber, pageSize, desc);
            return Ok(doctors);
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        // POST: api/Doctor
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(DoctorDto doctorDto)
        {
            var createdDoctor = await _doctorService.CreateAsync(doctorDto);
            return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctor.Id }, createdDoctor);
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorDto doctorDto)
        {
            if (id != doctorDto.Id)
            {
                return BadRequest();
            }

            var updatedDoctor = await _doctorService.UpdateAsync(doctorDto);
            if (updatedDoctor == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
