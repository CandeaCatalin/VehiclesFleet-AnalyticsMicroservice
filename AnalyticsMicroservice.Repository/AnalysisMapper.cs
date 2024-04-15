using AnalyticsMicroservice.DataAccess.Entities;
using AnalyticsMicroservice.Repository.Contracts;

namespace AnalyticsMicroservice.Repository;

public class AnalysisMapper:IAnalysisMapper
{
    public Domain.Entities.VehicleAnalysis DataAccessToDomain(VehicleAnalysis dataAccessVehicleAnalytics)
    {
        return new Domain.Entities.VehicleAnalysis
        {
            Id = dataAccessVehicleAnalytics.Id,
            VehicleId = dataAccessVehicleAnalytics.VehicleId,
            UserId = dataAccessVehicleAnalytics.UserId,
            MinimumSpeed = dataAccessVehicleAnalytics.MinimumSpeed,
            MaximumSpeed = dataAccessVehicleAnalytics.MaximumSpeed,
            AverageSpeed = dataAccessVehicleAnalytics.AverageSpeed,
            FuelConsumption = dataAccessVehicleAnalytics.FuelConsumption,
            HasTierPressureAnomaly = dataAccessVehicleAnalytics.HasTierPressureAnomaly,
            TotalKilometersPassed = dataAccessVehicleAnalytics.TotalKilometersPassed
        };
    }
}