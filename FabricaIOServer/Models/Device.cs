namespace FabricaIOServer.Models;

public class Device
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string keywords { get; set; }
    public required string description { get; set; }
    public required int type { get; set; }
    public required string DeviceJson { get; set; }
}