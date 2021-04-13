using System.Collections.Generic;

namespace DominandoEFCore.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        // prop. de navegação (1 departamento tem muitos funcionários)
        public List<Funcionario> Funcionarios { get; set; }
    }
}