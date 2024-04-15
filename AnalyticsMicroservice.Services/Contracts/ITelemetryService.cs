

using AnalyticsMicroservice.DataAccess.Entities;

namespace AnalyticsMicroservice.Services.Contracts;

public interface ITelemetryService
{
    public Task<IList<VehicleTelemetry>> GenerateAnalysisForVehicle(Guid vehicleId, string? token);
    
}