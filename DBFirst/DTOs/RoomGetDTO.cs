namespace DBFirst.DTOs;

public class RoomGetDTO
{
    public string Id { get; set; } = String.Empty;
    public bool HasTV { get; set; }
    public WardGetDTO Ward { get; set; }
}