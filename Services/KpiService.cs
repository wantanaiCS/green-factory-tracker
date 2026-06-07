using GreenFactory.Data;
using GreenFactory.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenFactory.Services;

public interface IKpiService
{
    Task<List<KpiTarget>> GetAllAsync();
    Task<KpiTarget?> GetByMeterAsync(string meterName);
    Task UpsertAsync(KpiTarget target);
}

public class KpiService : IKpiService
{
    private readonly AppDbContext _db;

    public KpiService(AppDbContext db) => _db = db;

    public Task<List<KpiTarget>> GetAllAsync()
        => _db.KpiTargets.OrderBy(x => x.MeterName).ToListAsync();

    public Task<KpiTarget?> GetByMeterAsync(string meterName)
        => _db.KpiTargets.FirstOrDefaultAsync(x => x.MeterName == meterName);

    public async Task UpsertAsync(KpiTarget target)
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