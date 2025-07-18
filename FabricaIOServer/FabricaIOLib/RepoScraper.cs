using FabricaIOServer.Models;
using Newtonsoft.Json;

namespace FabricaIOServer.FabricaIOLib;

/// <summary>
/// Used to scrape info from repositories
/// </summary>
public class RepoScraper
{
    /// <summary>
    /// The database context to store devices in
    /// </summary>
    private readonly DeviceContext _context;

    /// <summary>
    /// The org and app names
    /// </summary>
    private string _org, _AppName;

    /// <summary>
    /// The period, in hours, to repeat scraping the repositories
    /// </summary>
    private int _period;

    /// <summary>
    /// Creates a new repo scrapper
    /// </summary>
    /// <param name="context">the database context to use</param>
    /// <param name="org">The name or the organization to scrape repos for</param>
    /// <param name="AppName">The name of the Github registered app using the API</param>
    /// <param name="period">The period, in hours, to repeat the scraping</param>
    public RepoScraper(DeviceContext context, string org, string AppName, int period = 6)
    {
        _context = context;
        _org = org;
        _AppName = AppName;
        _period = period;
    }

    /// <summary>
    /// Infinite loop that scrapes repositories every period
    /// </summary>
    public async void scraperLoop()
    {
        TimeSpan delay = new(_period, 0, 0);
        while (true)
        {
            await updateDatabase();
            Console.WriteLine("Scraper sleeping for " + _period.ToString() + " hours");
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
        GitHubAPI api = new(_AppName);
        IReadOnlyList<string> jsons = await api.getLibraryJsons(await api.getRepos(_org));
        foreach (string json in jsons)
        {
            FabricaIODevice? device = JsonConvert.DeserializeObject<FabricaIODevice>(json);
            if (device?.fabricaio != null)
            {
                var exists = _context.Devices.Where(d => d.name == device.name);
                if (exists.Count() > 0)
                {
                    Device entry = exists.First();
                    entry.DeviceJson = JsonConvert.SerializeObject(device);
                    entry.keywords = device.keywords;
                    entry.type = device.fabricaio.type;
                    entry.version = device.version;
                    entry.description = device.description;
                }
                else
                {
                    _context.Add(new Device { name = device.name, keywords = device.keywords, version = device.version, description = device.description, type = device.fabricaio.type, DeviceJson = JsonConvert.SerializeObject(device) });
                }
                await _context.SaveChangesAsync();
            }
        }
        return true;
    }

}
