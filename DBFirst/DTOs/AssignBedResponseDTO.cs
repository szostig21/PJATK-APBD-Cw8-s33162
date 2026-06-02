namespace DBFirst.DTOs;

public class AssignBedResponseDTO
{
    public int Id { get; set; }
    public string PatientPesel { get; set; } = String.Empty;
    public int BedId { get; set; }
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
}