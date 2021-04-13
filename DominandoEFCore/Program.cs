using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            GapDoEnsureCreated();
        }

        static void HealthCheckBancoDeDados()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // método que valida a conexão com o BD
            var canConnect = db.Database.CanConnect();

            // não precisa mais do try/catch
            if (canConnect)
            {
                Console.WriteLine("Posso me conectar!");
            }
            else
            {
                Console.WriteLine("Não Posso me conectar!");
            }
        }

        static void EnsureCreatedAndDeleted()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }

        static void GapDoEnsureCreated()
        {
            using var db1 = new DominandoEFCore.Data.ApplicationContext();
            using var db2 = new DominandoEFCore.Data.ApplicationContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            // resolvendo o GAP do EnsureCreated
            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables(); // forçando a criação
        }
    }
}
