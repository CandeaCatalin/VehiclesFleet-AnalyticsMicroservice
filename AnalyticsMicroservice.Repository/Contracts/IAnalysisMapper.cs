using AnalyticsMicroservice.Domain.Entities;

namespace AnalyticsMicroservice.Repository.Contracts;

public interface IAnalysisMapper
{
    
    VehicleAnalysis DataAccessToDomain(AnalyticsMicroservice.DataAccess.Entities.VehicleAnalysis dataAccessVehicleAnalytics);
}