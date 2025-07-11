namespace FabricaIOServer.FabricaIOLib
{
    using System.Threading.Tasks;
    using Octokit;

    /// <summary>
    /// Provides methods for accessing and reading the contents of github repositories.
    /// </summary>
    class GitHubAPI
    {
        /// <summary>
        /// The github client object used
        /// </summary>
        private GitHubClient client;

        /// <summary>
        /// Creates a new GitHubAPI object
        /// </summary>
        /// <param name="AppName">The name of the Github registered app using the API</param>
        public GitHubAPI(string AppName)
        {
            client = new GitHubClient(new ProductHeaderValue(AppName));
            // Temporary using user secrets to store APi token
            var builder = WebApplication.CreateBuilder();
            Credentials token = new Credentials(builder.Configuration["GitHubAPItoken"]);
            client.Credentials = token;
        }

        /// <summary>
        /// Gets all repos for Fabrica-IO Org
        /// </summary>
        /// <returns>A list of the repos</returns>
        public async Task<IReadOnlyList<Repository>> getRepos()
        {
            return await client.Repository.GetAllForOrg("FabricaIO");
        }

        /// <summary>
        /// Gets all the library.json files from a list of repositories. If one doesn't exist, it's skipped.
        /// </summary>
        /// <param name="repos">A List of repositories</param>
        /// <returns>A list of the text contents of library.json values</returns>
        public async Task<IReadOnlyList<string>> getLibraryJsons(IReadOnlyList<Repository> repos)
        {
            List<string> libraries = new();
            foreach (var repo in repos)
            {
                try
                {
                    IReadOnlyList<RepositoryContent> library = await client.Repository.Content.GetAllContents(repo.Id, "library.json");
                    string? library_text = library.FirstOrDefault()?.Content;
                    if (library_text != null)
                    {
                        libraries.Add(library_text);
                    }
                }
                catch (NotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (RateLimitExceededException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return libraries;
        }
    }
}