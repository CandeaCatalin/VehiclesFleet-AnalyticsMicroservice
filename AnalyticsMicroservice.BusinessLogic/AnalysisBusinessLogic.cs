using AnalyticsMicroservice.BusinessLogic.Contracts;
using AnalyticsMicroservice.DataAccess.Entities;
using AnalyticsMicroservice.Domain.Dtos;
using AnalyticsMicroservice.Repository.Contracts;
using AnalyticsMicroservice.Services.Contracts;

namespace AnalyticsMicroservice.BusinessLogic;

public class AnaysisBulsinessLogic: IAnalysisBusinessLogic
{
    private readonly ITelemetryService telemetryService;
    private readonly IAnalysisRepository analysisRepository;
    private readonly ILoggerService loggerService;
    public AnaysisBulsinessLogic(ITelemetryService telemetryService,IAnalysisRepository analysisRepository,ILoggerService loggerService)
    {
        this.telemetryService = telemetryService;
        this.analysisRepository = analysisRepository;
        this.loggerService = loggerService;
    }
    public async Task GenerateAnalysisForVehicle(GetAnalysisDto dto, string? token)
    {
        if (dto.VehicleId.Equals(new Guid()))
        {
            throw new Exception("Vehicle Id is null");
        }
        var telemetriesForVehicle = await telemetryService.GenerateAnalysisForVehicle(dto.VehicleId,token);
        var eligibleTelemetries = GetEligibleTelemetries(telemetriesForVehicle);
        
        if (eligibleTelemetries.Count() != 0)
        {
            var vehicleAnalysis = new VehicleAnalysis
            {
                Id = Guid.NewGuid(),
                VehicleId = dto.VehicleId,
                UserId = dto.UserId.ToString(),
                MinimumSpeed = GetMinimumSpeed(eligibleTelemetries),
                MaximumSpeed = GetMaximumSpeed(eligibleTelemetries),
                AverageSpeed = GetAverageSpeed(eligibleTelemetries),
                FuelConsumption = GetFuelConsumption(eligibleTelemetries),
                HasTierPressureAnomaly = ArePressureAnomalies(eligibleTelemetries),
                TotalKilometersPassed = telemetriesForVehicle.LastOrDefault()!.KilometersSinceStart
            };
            await analysisRepository.AddAnalysis(vehicleAnalysis);
            await loggerService.LogInfo($"Analysis added for vehicle with Id: {dto.VehicleId}", token);
        }
    }
    public async Task<IList<Domain.Entities.VehicleAnalysis>> GetAnalyticsForVehicle(Guid vehicleId)
    {
        return await analysisRepository.GetAnalyticsForVehicle(vehicleId);
    }
    
    private IList<VehicleTelemetry> GetEligibleTelemetries(IList<VehicleTelemetry> telemetries)
    {
        var filteredList = telemetries.Where(v => v.ActualSpeed == 0).ToList();
        if (filteredList.Count >= 2)
        {
            var last = filteredList.LastOrDefault();
            var first = filteredList.ElementAt(filteredList.Count - 2);
         
            int lastIndex =
                telemetries.IndexOf(last); 
            int secondLastIndex = telemetries.IndexOf(first);

            if (lastIndex != -1 && secondLastIndex != -1)
            {
            
                int startIndex = Math.Min(lastIndex, secondLastIndex);
                int endIndex = Math.Max(lastIndex, secondLastIndex);

                return telemetries.Skip(startIndex).Take(endIndex - startIndex + 1)
                    .ToList();
            }
        }

        return new List<VehicleTelemetry>();
    }
    
    private bool ArePressureAnomalies(IList<VehicleTelemetry> telemetries)
    {
        var tirePressure = telemetries.Select(x => x.TirePressure).ToList();
        return tirePressure.Max() - tirePressure.Min() >= 10;
    }

    private int GetMinimumSpeed(IList<VehicleTelemetry> telemetries)
    {
        return telemetries.Where(e => e.ActualSpeed != 0).Min(e => e.ActualSpeed);
    }
    
    private int GetMaximumSpeed(IList<VehicleTelemetry> telemetries)
    {
        return telemetries.Where(e => e.ActualSpeed != 0).Max(e => e.ActualSpeed);
    }

    private double GetAverageSpeed(IList<VehicleTelemetry> telemetries)
    {
        return telemetries.Where(t=>t.ActualSpeed != 0)
            .Average(e => e.ActualSpeed);
    }
    
    private double GetFuelConsumption(IList<VehicleTelemetry> telemetries)
    {
        var fuels = telemetries.Select(x => x.Fuel).ToList();
                    
        var maxFuel = fuels.ElementAt(0);
        var minFuel = maxFuel;
        var fuelConsumption = 0.0;
        foreach (var f in fuels)
        {
            if (f > minFuel)
            {
                fuelConsumption = (double)(maxFuel - minFuel);
                maxFuel = f;
                minFuel = f;
            }
            else
            {
                minFuel = f;
            }
        }

        if (fuelConsumption == 0)
        {
            fuelConsumption = (double)(maxFuel - minFuel);
        }

        return fuelConsumption;
    }
}