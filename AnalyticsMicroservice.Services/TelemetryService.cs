using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AnalyticsMicroservice.DataAccess.Entities;

using AnalyticsMicroservice.Services.Contracts;
using AnalyticsMicroservice.Settings;

namespace AnalyticsMicroservice.Services;

public class TelemetryService: ITelemetryService
{
    private readonly IAppSettingsReader appSettingsReader;
    private readonly HttpClient httpClient;

    public TelemetryService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
        var clientHandler = new HttpClientHandler();
        httpClient = new HttpClient(clientHandler);
    }

    public async Task<IList<VehicleTelemetry>> GenerateAnalysisForVehicle(Guid vehicleId, string? token)
    {
        IList<VehicleTelemetry> telemetryList = new List<VehicleTelemetry>();
        if (token is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
       
        
        var logInfoUrl = appSettingsReader.GetValue(AppSettingsConstants.Section.TelemetryMicroserviceSectionName, AppSettingsConstants.Keys.GetTelemetriesUrlKey);

        var response = await httpClient.PostAsync(logInfoUrl, GetHttpContent(vehicleId));
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
       telemetryList = JsonSerializer.Deserialize<IList<VehicleTelemetry>>(responseBody,options) ?? throw new InvalidOperationException();

        return telemetryList;
    }
    private HttpContent GetHttpContent(Guid vehicleId)
    {
        var content = JsonSerializer.Serialize(new
        { 
            VehicleId = vehicleId
        });

        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}