using System;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            EnsureCreatedAndDeleted();
        }

        static void EnsureCreatedAndDeleted()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }
    }
}
