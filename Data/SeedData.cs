using GreenFactory.Models;

namespace GreenFactory.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.EnergyRecords.Any()) return;

        var random = new Random(42);
        var meters = new[] { "Line A", "Line B", "Line C" };
        var records = new List<EnergyRecord>();

        // ข้อมูลย้อนหลัง 30 วัน
        for (int i = 30; i >= 0; i--)
        {
            var date = DateTime.Today.AddDays(-i);
            foreach (var meter in meters)
            {
                double kwh = Math.Round(random.NextDouble() * 400 + 200, 2); // 200-600
                double water = Math.Round(random.NextDouble() * 40 + 10, 2);   // 10-50
                double carbon = Math.Round((kwh * 0.4999) + (water * 0.708), 2);

                records.Add(new EnergyRecord
                {
                    Timestamp = date.AddHours(8),
                    MeterName = meter,
                    kWh = kwh,
                    WaterM3 = water,
                    CarbonKgCO2e = carbon
                });
            }
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