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
            // SqlInjection();
            // MigracoesPendentes();
            // AplicarMigracaoEmTempoDeExecucao();
            // TodasMigrações();
            // MigracoesJaAplicadas();
            ScriptGeralDoBancoDeDados();
        }

        static void ScriptGeralDoBancoDeDados()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();
            var script = db.Database.GenerateCreateScript();

            System.Console.WriteLine(script);
        }

        static void MigracoesJaAplicadas()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // recupera as migrações aplicadas no BD
            var migracoes = db.Database.GetAppliedMigrations();

            // imprime na tela o total de migrações
            Console.WriteLine($"Total: {migracoes.Count()}");

            // exibe as migrações
            foreach(var migracao in migracoes)
            {
                System.Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void TodasMigrações()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // método que recupera todas as migrações da aplicação em tempo de execução
            var migracoes = db.Database.GetMigrations();

            // imprime na tela o total de migrações
            Console.WriteLine($"Total: {migracoes.Count()}");

            // exibe as migrações
            foreach(var migracao in migracoes)
            {
                System.Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void AplicarMigracaoEmTempoDeExecucao()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // detecta todas migrações pendentes da aplicação e executa-las
            db.Database.Migrate();
        }

        static void MigracoesPendentes()
        {
            using var db = new DominandoEFCore.Data.ApplicationContext();

            // recupera as migrações pendentes
            var migracoesPendentes = db.Database.GetPendingMigrations();

            System.Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracao in migracoesPendentes)
            {
                System.Console.WriteLine($"Migração: {migracao}");
            }
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
            // var descricao = "Departamento 1";
            // db.Database.ExecuteSqlRaw("update departamentos set descricao='DepartamentoAlterado' where descricao={0}", descricao);

            // atualizando um registro - de forma insegura
            var descricao2 = "Teste ' or 1='1";
            db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao2}'");

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
