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

    // 🇹🇭 ฟังก์ชันช่วยแปลงเวลาให้เป็นเขตเวลาประเทศไทย (GMT+7) เสมอ เพื่อความแม่นยำ
    private DateTime GetThailandToday()
    {
        var utcNow = DateTime.UtcNow;
        var thailandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var thailandTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, thailandTimeZone);
        return thailandTime.Date;
    }

    public async Task<List<EnergyRecord>> GetLast30DaysAsync(string? meter = null)
    {
        var query = _db.EnergyRecords.AsQueryable();
        if (meter != null) query = query.Where(r => r.MeterName == meter);

        var todayTh = GetThailandToday();
        return await query
            .Where(r => r.Timestamp >= todayTh.AddDays(-30))
            .OrderBy(r => r.Timestamp)
            .ToListAsync();
    }

    public async Task<double> GetTodayTotalKwhAsync()
    {
        var todayTh = GetThailandToday();
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == todayTh) // ✅ เช็กตามวันเวลาของไทย
            .SumAsync(r => r.kWh);
    }

    public async Task<double> GetTodayTotalWaterAsync()
    {
        var todayTh = GetThailandToday();
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == todayTh) // ✅ เช็กตามวันเวลาของไทย
            .SumAsync(r => r.WaterM3);
    }

    public async Task<double> GetTodayTotalCarbonAsync()
    {
        var todayTh = GetThailandToday();
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == todayTh) // ✅ เช็กตามวันเวลาของไทย
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
        var todayTh = GetThailandToday();
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == todayTh)
            .ToListAsync() // 👈 ดึงมาก่อนเพื่อหลีกเลี่ยงข้อจำกัดการ Group ของบาง DB
            .ContinueWith(t => t.Result
                .GroupBy(r => r.MeterName.Trim()) // 👈 ตัดช่องว่างออก
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(r => r.kWh),
                    StringComparer.OrdinalIgnoreCase // 👈 พิมพ์เล็กพิมพ์ใหญ่ถือว่าเป็นตัวเดียวกัน
                ));
    }

    public async Task<Dictionary<string, double>> GetTodayWaterByMeterAsync()
    {
        var todayTh = GetThailandToday();
        return await _db.EnergyRecords
            .Where(r => r.Timestamp.Date == todayTh)
            .ToListAsync() // 👈 ดึงมาก่อน
            .ContinueWith(t => t.Result
                .GroupBy(r => r.MeterName.Trim()) // 👈 ตัดช่องว่างออก
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(r => r.WaterM3),
                    StringComparer.OrdinalIgnoreCase // 👈 พิมพ์เล็กพิมพ์ใหญ่ถือว่าเป็นตัวเดียวกัน
                ));
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

    public async Task<double> GetSettingAsync(string key, double defaultValue)
    {
        var setting = await _db.AppSettings.FirstOrDefaultAsync(x => x.Key == key);
        return setting?.Value ?? defaultValue;
    }

    public async Task UpsertSettingAsync(string key, double value, string description = "")
    {
        var existing = await _db.AppSettings.FirstOrDefaultAsync(x => x.Key == key);
        if (existing is null)
            _db.AppSettings.Add(new AppSetting { Key = key, Value = value, Description = description });
        else
            existing.Value = value;

        await _db.SaveChangesAsync();
    }
}