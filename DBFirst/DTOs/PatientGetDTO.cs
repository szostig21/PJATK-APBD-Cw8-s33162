namespace DBFirst.DTOs;

public class PatientGetDTO
{
    public string Pesel { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public int Age { get; set; }
    public string Sex  { get; set; } = String.Empty;

    public IEnumerable<AdmissionGetDTO> Admissions { get; set; }
    public IEnumerable<BedAssignmentsDTO> BedAssignments { get; set; }
    

}