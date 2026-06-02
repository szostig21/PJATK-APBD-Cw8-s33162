namespace DBFirst.DTOs;

public class BedGetDTO
{
    public int Id { get; set; }
    public BedTypeGetDTO BedType { get; set; }
    public RoomGetDTO Room { get; set; }
}