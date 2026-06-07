using GreenFactory.Data;
using GreenFactory.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenFactory.Services;

public class EnergyService
{
    private readonly AppDbContext _db;

    public EnergyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<EnergyRecord>> GetLast30DaysAsync(string? meter = null)
    {
        var query = _db.EnergyRecords.AsQueryable();
        if (meter != null) query = query.Where(r => r.MeterName == meter);
        return await query
            .Where(r => r.Timestamp >= DateTime.Now.AddDays(-30))
            .OrderBy(r => r.Timestamp)
            .ToListAsync();
    }

    public async Task<double> GetTodayTotalKwhAsync()
    {
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == DateTime.Today)
            .SumAsync(r => r.kWh);
    }

    public async Task<double> GetTodayTotalWaterAsync()
    {
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == DateTime.Today)
            .SumAsync(r => r.WaterM3);
    }

    public async Task<double> GetTodayTotalCarbonAsync()
    {
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == DateTime.Today)
            .SumAsync(r => r.CarbonKgCO2e);
    }

    public async Task<List<KpiTarget>> GetKpiTargetsAsync()
    {
        return await _db.KpiTargets.ToListAsync();
    }
}