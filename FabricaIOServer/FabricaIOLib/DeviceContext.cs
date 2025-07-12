using Microsoft.EntityFrameworkCore;
using FabricaIOServer.Models;

namespace FabricaIOServer.FabricaIOLib;

public class DeviceContext : DbContext
{
    public DbSet<Device> Devices { get; set; } = null!;

    public DeviceContext(DbContextOptions<DeviceContext> options) : base(options) { }

}
