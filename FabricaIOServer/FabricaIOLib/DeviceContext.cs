using Microsoft.EntityFrameworkCore;
using FabricaIOServer.Models;

namespace FabricaIOServer.FabricaIOLib;

public class DeviceContext : DbContext
{
    /// <summary>
    /// Provides collection style interface for database
    /// </summary>
    public DbSet<Device> Devices { get; set; } = null!;

    /// <summary>
    /// Creates a new data base context for the device database
    /// </summary>
    /// <param name="options">The options for the context</param>
    public DeviceContext(DbContextOptions<DeviceContext> options) : base(options) { }

}
