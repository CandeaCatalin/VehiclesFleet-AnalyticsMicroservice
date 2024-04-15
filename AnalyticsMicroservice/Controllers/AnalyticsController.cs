using AnalyticsMicroservice.BusinessLogic.Contracts;
using AnalyticsMicroservice.Domain.Dtos;
using AnalyticsMicroservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AnalyticsMicroservice.Controllers;

[ApiController]
[Route("analytics")]
public class AnalyticsController: ControllerBase
{
    private readonly IAnalysisBusinessLogic analysisBusinessLogic;

    public AnalyticsController(IAnalysisBusinessLogic analysisBusinessLogic)
    {
        this.analysisBusinessLogic = analysisBusinessLogic;
    }
    
    [HttpPost("AddAnalytics")]
    public async Task<IActionResult> GetAnalysisForVehicle(GetAnalysisDto dto)
    {
        var token = GetToken();
        await analysisBusinessLogic.GenerateAnalysisForVehicle(dto,token);
        return Ok();
    }
    
    [HttpPost("getForVehicle")]
    public async Task<IList<VehicleAnalysis>> GetAnalyticsForVehicle(Guid vehicleId)
    {
        return await analysisBusinessLogic.GetAnalyticsForVehicle(vehicleId);
    }

    
    
    private string? GetToken()
    {
        if (Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue))
        {
            var token = authHeaderValue.ToString().Replace("Bearer ", "");
              
            return token;
        }

        return null;
    }
}