using System;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=dominando-efcore;Integrated Security=True;Polling=true";
            optionsBuilder
                .UseSqlServer(strConnection) // usando sqlServer
                .EnableSensitiveDataLogging() // habilita dados sensiveis - valores de parâmetros
                .LogTo(Console.WriteLine, LogLevel.Information); // logando no console os comandos executados pelo EFCore
        }
    }
}