using Microsoft.EntityFrameworkCore;
using GreenFactory.Models;

namespace GreenFactory.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EnergyRecord> EnergyRecords => Set<EnergyRecord>();
    public DbSet<KpiTarget> KpiTargets => Set<KpiTarget>();
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();
}