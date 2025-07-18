namespace FabricaIOServer.FabricaIOLib;

/// <summary>
/// Describes the minimum required library.json for Fabrica-IO devices as an object
/// </summary>
public class FabricaIODevice
{
    public required string name, version, description, keywords, license, homepage, frameworks, platforms;
    public required RepositoryInfo repository;
    public required List<Author> authors;
    public required Fabricaio fabricaio;
}

public class Author {
    public required string name, email, url;
}

public class Constructor {
    public required string name, type, description, @default;
}

public class Fabricaio
{
    public required string name, category, libname, description;
    public int type;
    public required string[] includes;
    public required List<Constructor[]> constructor;
}

public class RepositoryInfo
{
    public required string type, url;
};