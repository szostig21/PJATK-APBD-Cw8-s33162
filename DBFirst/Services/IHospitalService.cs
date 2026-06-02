using DBFirst.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DBFirst.Services;

public interface IHospitalService
{
    Task<IEnumerable<PatientGetDTO>> GetPatients(string? search, CancellationToken ct);
    Task AssignBeds(string pesel, AssignBedRequestDTO request, CancellationToken ct);
}