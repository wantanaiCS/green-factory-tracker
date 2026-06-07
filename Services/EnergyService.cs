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

    public async Task<List<EnergyRecord>> GetRecentRecordsAsync(int limit = 50)
    {
        return await _db.EnergyRecords
            .OrderByDescending(r => r.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Dictionary<string, double>> GetTodayKwhByMeterAsync()
    {
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == DateTime.Today)
            .GroupBy(r => r.MeterName)
            .Select(g => new { Meter = g.Key, Total = g.Sum(r => r.kWh) })
            .ToDictionaryAsync(x => x.Meter, x => x.Total);
    }

    public async Task<Dictionary<string, double>> GetTodayWaterByMeterAsync()
    {
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == DateTime.Today)
            .GroupBy(r => r.MeterName)
            .Select(g => new { Meter = g.Key, Total = g.Sum(r => r.WaterM3) })
            .ToDictionaryAsync(x => x.Meter, x => x.Total);
    }

    public async Task UpsertKpiTargetAsync(KpiTarget target)
    {
        var existing = await _db.KpiTargets
            .FirstOrDefaultAsync(x => x.MeterName == target.MeterName);

        if (existing is null)
            _db.KpiTargets.Add(target);
        else
        {
            existing.EnergyTarget = target.EnergyTarget;
            existing.EnergyThreshold = target.EnergyThreshold;
            existing.WaterTarget = target.WaterTarget;
            existing.WaterThreshold = target.WaterThreshold;
        }

        await _db.SaveChangesAsync();
    }
}