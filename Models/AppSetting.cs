namespace GreenFactory.Models;

public class AppSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public double Value { get; set; }
    public string Description { get; set; } = "";
}