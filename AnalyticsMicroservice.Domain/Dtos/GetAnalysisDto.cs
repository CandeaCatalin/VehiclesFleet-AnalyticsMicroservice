namespace AnalyticsMicroservice.Domain.Dtos;

public class GetAnalysisDto
{
    public Guid VehicleId { get; set; }
    public Guid UserId { get; set; }
}