using AnalyticsMicroservice.DataAccess;
using AnalyticsMicroservice.DataAccess.Entities;
using AnalyticsMicroservice.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsMicroservice.Repository;

public class AnalysisRepository: IAnalysisRepository
{
    private readonly DataContext dataContext;
    private readonly IAnalysisMapper analysisMapper;

    public AnalysisRepository(DataContext dataContext,IAnalysisMapper analysisMapper)
    {
        this.dataContext = dataContext;
        this.analysisMapper = analysisMapper;
    }
    public async Task AddAnalysis(VehicleAnalysis analysis)
    {
        if (analysis is null)
        {
            throw new Exception("Analysis cannot be null");
        }

        await dataContext.Vehicles.AddAsync(new Vehicle { Id = analysis.VehicleId });
        
        await dataContext.VehiclesAnalysis.AddAsync(analysis);
        await dataContext.SaveChangesAsync();
    }
    public async Task<IList<Domain.Entities.VehicleAnalysis>> GetAnalyticsForVehicle(Guid vehicleId)
    {
        var dataAccessVehicles =  await dataContext.VehiclesAnalysis.Where(vh => vh.VehicleId == vehicleId).ToListAsync();
        var domainVehicles = new List<Domain.Entities.VehicleAnalysis>();

        foreach (var v in dataAccessVehicles)
        {
            domainVehicles.Add(analysisMapper.DataAccessToDomain(v));
        }

        return domainVehicles;
    }
}