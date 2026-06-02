namespace DBFirst.DTOs;

public class BedAssignmentsDTO
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public BedGetDTO Bed { get; set; }
}