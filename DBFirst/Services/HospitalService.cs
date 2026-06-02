using DBFirst.DTOs;
using DBFirst.Exceptions;
using DBFirst.Infrastructure;
using DBFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Services;

public class HospitalService(HospitalDbContext hdc) : IHospitalService
{
    public async Task<IEnumerable<PatientGetDTO>> GetPatients(string? search, CancellationToken ct)
    {
        var query = hdc.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search}%";
            
            query = query.Where(p => 
                EF.Functions.Like(p.FirstName, pattern) || 
                EF.Functions.Like(p.LastName, pattern));
        }
        
        return await query.Select(p => new PatientGetDTO
        {
            Pesel =  p.Pesel,
            FirstName = p.FirstName,
            LastName =  p.LastName,
            Age =  p.Age,
            Sex = p.Sex ? "Male" : "Female",
            
            Admissions = p.Admissions.Select(a => new AdmissionGetDTO
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new WardGetDTO
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }
            ).ToList(),
            
            BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentsDTO
            {
                Id = ba.Id,
                From = ba.From,
                To = ba.To,
                Bed = new BedGetDTO
                {
                    Id = ba.Bed.Id,
                    BedType = new BedTypeGetDTO
                    {
                        Id = ba.Bed.BedType.Id,
                        Name = ba.Bed.BedType.Name,
                        Description = ba.Bed.BedType.Description
                    },
                    Room = new RoomGetDTO
                    {
                        Id = ba.Bed.Room.Id,
                        HasTV = ba.Bed.Room.HasTv,
                        Ward = new WardGetDTO
                        {
                            Id = ba.Bed.Room.Ward.Id,
                            Name = ba.Bed.Room.Ward.Name,
                            Description = ba.Bed.Room.Ward.Description,
                        }
                    }
                }
            }).ToList(),
        }).ToListAsync(ct);
    }

    public async Task AssignBeds(string pesel, AssignBedRequestDTO request, CancellationToken ct)
    {
        var patientExist = await hdc.Patients.AnyAsync(p => p.Pesel == pesel, ct);

        if (!patientExist)
        {
            throw new NotFoundException("Patient with that pesel does not exist");
        }
        
        var requestedTo = request.To ?? DateTime.MaxValue;
        
        var occupiedBedIds = hdc.BedAssignments
            .Where(ba =>
                ba.From < requestedTo &&
                (ba.To ?? DateTime.MaxValue) > request.From)
            .Select(ba => ba.BedId);

        var freeBed = await hdc.Beds
            .Include(b => b.Room).ThenInclude(r => r.Ward)
            .Include(b => b.BedType)
            .Where(b => b.BedType.Name == request.BedType &&
                        b.Room.Ward.Name == request.Ward &&
                        !occupiedBedIds.Contains(b.Id)).FirstOrDefaultAsync(ct);
        if (freeBed is null)
        {
            throw new NotFoundException($"Brak wolnego lozka typu '{request.BedType}' na oddziale '{request.Ward}' w podanym terminie");
        }

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = freeBed.Id,
            From = request.From,
            To = request.To
        };

        hdc.BedAssignments.Add(assignment);
        await hdc.SaveChangesAsync(ct);
    }
}