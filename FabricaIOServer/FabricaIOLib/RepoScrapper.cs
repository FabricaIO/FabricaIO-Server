using FabricaIOServer.Models;
using Newtonsoft.Json;

namespace FabricaIOServer.FabricaIOLib;

/// <summary>
/// Used to scrape info from repositories
/// </summary>
public class RepoScrapper
{
    private readonly DeviceContext _context;

    public RepoScrapper(DeviceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Infinite loop that scrapes repositories every 24 hours
    /// </summary>
    public async void scrapperLoop()
    {
        TimeSpan delay = new(24, 0, 0);
        while (true)
        {
            await updateDatabase();
            Console.WriteLine("Sleeping for 24 hours");
            await Task.Delay(delay);
        }
    }

    /// <summary>
    /// Scrapes repos and updates database
    /// </summary>
    /// <returns>True when done</returns>
    private async Task<bool> updateDatabase()
    {
        Console.WriteLine("Scrapping repos...");
        GitHubAPI api = new("Fabrica-IO-Server");
        IReadOnlyList<string> jsons = await api.getLibraryJsons(await api.getRepos());
        foreach (string json in jsons)
        {
            FabricaIODevice? device = JsonConvert.DeserializeObject<FabricaIODevice>(json);
            if (device?.fabricaio != null)
            {
                var exists = _context.Devices.Where(d => d.name == device.name);
                if (exists.Count() > 0)
                {
                    var entry = exists.First();
                    entry.DeviceJson = JsonConvert.SerializeObject(device);
                    entry.keywords = device.keywords;
                    entry.type = device.fabricaio.type;
                    entry.description = device.description;
                }
                else
                {
                    _context.Add(new Device { name = device.name, keywords = device.keywords, description = device.description, type = device.fabricaio.type, DeviceJson = JsonConvert.SerializeObject(device) });
                }
                await _context.SaveChangesAsync();
            }
        }
        return true;
    }

}
