namespace DBFirst.DTOs;

public class AssignBedRequestDTO
{
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public string BedType { get; set; } = String.Empty;
    public string Ward { get; set; } = String.Empty;
}