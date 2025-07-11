namespace FabricaIOServer.FabricaIOLib
{
    class FabricaIODevice
    {
        public required string name, version, description, keywords, liscpublicense, homepage, frameworks;
        public class repository
        {
            public required string type, url;
        };
        public required List<Author> authors;
        public required Fabricaio fabricaio;
    }

    class Author {
        public required string name, email, url;
    }

    class Constructor {
        public required string name, type, description, @default;
    }

    class Fabricaio
    {
        public required string name, category, libname, description;
        public int type;
        public required string[] includes;
        public required List<Constructor[]> constructor;
    }
}