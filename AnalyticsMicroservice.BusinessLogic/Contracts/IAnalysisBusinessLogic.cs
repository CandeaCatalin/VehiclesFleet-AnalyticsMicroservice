using AnalyticsMicroservice.Domain.Dtos;
using AnalyticsMicroservice.Domain.Entities;

namespace AnalyticsMicroservice.BusinessLogic.Contracts;

public interface IAnalysisBusinessLogic
{
    public Task GenerateAnalysisForVehicle(GetAnalysisDto dto,string? token);
    public Task<IList<VehicleAnalysis>> GetAnalyticsForVehicle(Guid vehicleId);

}