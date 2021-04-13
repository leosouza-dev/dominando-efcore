using System;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContextCidade : DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=dominando-efcore;Integrated Security=True;Pooling=true";
            optionsBuilder
                .UseSqlServer(strConnection) // usando sqlServer
                .EnableSensitiveDataLogging() // habilita dados sensiveis - valores de parâmetros
                .LogTo(Console.WriteLine, LogLevel.Information); // logando no console os comandos executados pelo EFCore
        }
    }
}