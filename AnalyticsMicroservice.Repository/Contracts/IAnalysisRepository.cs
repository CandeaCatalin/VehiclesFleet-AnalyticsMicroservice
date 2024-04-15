using AnalyticsMicroservice.DataAccess.Entities;

namespace AnalyticsMicroservice.Repository.Contracts;

public interface IAnalysisRepository
{
    public Task AddAnalysis(VehicleAnalysis analysis);
    Task<IList<Domain.Entities.VehicleAnalysis>> GetAnalyticsForVehicle(Guid vehicleId);
}