namespace GreenFactory.Models;

public class EnergyRecord
{
	public int Id { get; set; }
	public DateTime Timestamp { get; set; }
	public string MeterName { get; set; } = "";
	public double kWh { get; set; }
	public double WaterM3 { get; set; }
	public double CarbonKgCO2e { get; set; }
}