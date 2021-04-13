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
            // GapDoEnsureCreated();
            //HealthCheckBancoDeDados();
            GerenciarEstadoDaConexao();
        }

        static void GerenciarEstadoDaConexao()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            var time  = System.Diagnostics.Stopwatch.StartNew(); // iniciando a contagem

            // realizando 200 consultas
            for (int i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            // para a contagem de tempo após as 200 consultas
            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}";

            Console.WriteLine(mensagem);
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
