using AnalyticsMicroservice.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsMicroservice.DataAccess;

public class DataContext:DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
    public DbSet<VehicleAnalysis> VehiclesAnalysis { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
}