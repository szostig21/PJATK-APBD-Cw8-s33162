using DBFirst.DTOs;
using DBFirst.Exceptions;
using DBFirst.Infrastructure;
using DBFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController(IHospitalService hospitalService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search, CancellationToken ct)
    {
        return Ok(await hospitalService.GetPatients(search, ct));
    }

    [HttpPost]
    public async Task<IActionResult> AssignBed(string pesel, [FromBody] AssignBedRequestDTO request,
        CancellationToken ct)
    {
        try
        {
            await hospitalService.AssignBeds(pesel, request, ct);
            return Created(string.Empty, null);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}