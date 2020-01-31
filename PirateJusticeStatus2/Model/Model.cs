using System;
using System.Linq;
using SiteLibrary;

namespace Publicus
{
    public static class Model
    {
        public static int CurrentVersion = 2;

        public static void Install(IDatabase database)
        {
            CreateAllTables(database);
            Migrate(database);
        }

        private static void CreateAllTables(IDatabase database)
        {
            database.CreateTable<Meta>();
            database.CreateTable<User>();
            database.CreateTable<Phrase>();
            database.CreateTable<PhraseTranslation>();
        }

        private static void Migrate(IDatabase database)
        {
            var meta = database.Query<Meta>().SingleOrDefault();

            if (meta == null)
            {
                meta = new Meta(Guid.NewGuid());
            }

            while (meta.Version.Value < CurrentVersion)
            {
                meta.Version.Value++;
                Migrate(database, meta.Version.Value);
            }

            database.Save(meta);
        }

        private static void Migrate(IDatabase database, int version)
        {
            switch (version)
            {
                case 1:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
