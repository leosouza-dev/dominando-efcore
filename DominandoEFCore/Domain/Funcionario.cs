namespace DominandoEFCore.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }

        // relacionamento (1 departamento tem muitos funcionários
        // e 1 funcionário possui 1 departamento - n:1)
        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; } // prop. de navegação
    }
}