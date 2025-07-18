namespace FabricaIOServer.Models;

/// <summary>
/// Models a Fabrica-IO device at it appears in the database
/// </summary>
public class Device
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string version { get; set; }
    public required string keywords { get; set; }
    public required string description { get; set; }
    public required int type { get; set; }
    public required string DeviceJson { get; set; }
}