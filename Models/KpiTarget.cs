namespace GreenFactory.Models;

public class KpiTarget
{
    public int Id { get; set; }
    public string MeterName { get; set; } = "";
    public double EnergyTarget { get; set; }
    public double EnergyThreshold { get; set; }
    public double WaterTarget { get; set; }
    public double WaterThreshold { get; set; }
}