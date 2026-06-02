namespace DBFirst.DTOs;

public class AdmissionGetDTO
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public WardGetDTO Ward { get; set; }
}