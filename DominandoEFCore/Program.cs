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
            // HealthCheckBancoDeDados();
            // _count=0; // resetando
            // GerenciarEstadoDaConexao(false);
            // _count=0; // resetando
            // GerenciarEstadoDaConexao(true);
            SqlInjection();
        }

        static void SqlInjection()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Domain.Departamento
                {
                    Descricao = "Departamento 1"
                },
                new Domain.Departamento
                {
                    Descricao = "Departamento 2"
                }
            );
            db.SaveChanges();

            // atualizando um registro - de forma segura
            var descricao = "Departamento 1";
            db.Database.ExecuteSqlRaw("update departamentos set descricao='DepartamentoAlterado' where descricao={0}", descricao);

            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                System.Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
            }
        }

        static void ExecuteSQL()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // primeira opção - criando comando - não tão seguro
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            // modo mais seguro
            var descricao = "TESTE";
            db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id=1", descricao);

            // terceira opção
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id=1");
        }

        static int _count;
        static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            var time  = System.Diagnostics.Stopwatch.StartNew(); // iniciando a contagem

            //recuperando a SQLConnection
            var conexao = db.Database.GetDbConnection();

            //incrementar contador de conexão sempre que a conexão for alterada
            conexao.StateChange += (_, __) => ++ _count;
            
            if(gerenciarEstadoConexao)
            {
                conexao.Open();
            }

            // realizando 200 consultas
            for (int i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            // para a contagem de tempo após as 200 consultas
            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, {_count}";

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
