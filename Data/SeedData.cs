using GreenFactory.Models;

namespace GreenFactory.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.EnergyRecords.Any()) return;

        var random = new Random();
        var meters = new[] { "Line A", "Line B", "Line C" };
        var records = new List<EnergyRecord>();

        foreach (var meter in meters)
        {
            double kwh = random.NextDouble() * 500 + 200;
            double water = random.NextDouble() * 50 + 10;
            double carbon = (kwh * 0.4999) + (water * 0.708);

            records.Add(new EnergyRecord
            {
                Timestamp = DateTime.Today.AddHours(8),
                MeterName = meter,
                kWh = Math.Round(kwh, 2),
                WaterM3 = Math.Round(water, 2),
                CarbonKgCO2e = Math.Round(carbon, 2)
            });
        } 

        // KPI Targets
        var targets = meters.Select(m => new KpiTarget
        {
            MeterName = m,
            EnergyTarget = 400,
            EnergyThreshold = 600,
            WaterTarget = 30,
            WaterThreshold = 50
        });

        context.EnergyRecords.AddRange(records);
        context.KpiTargets.AddRange(targets);
        context.SaveChanges();
    }
}