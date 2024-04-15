namespace AnalyticsMicroservice.DataAccess.Entities;

public class VehicleTelemetry
{
    public int ActualSpeed { get; set; }
    public int KilometersSinceStart { get; set; }
    public decimal Fuel { get; set; }
    public float TirePressure { get; set; }
 }