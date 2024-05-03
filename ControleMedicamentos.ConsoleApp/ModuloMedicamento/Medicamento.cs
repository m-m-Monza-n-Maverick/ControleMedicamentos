using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento : EntidadeBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        protected DateTime DataValidade { get; set; }
        public int Quantidade { get; set; } = 5;

        public Medicamento(string nome, string descricao, string lote, DateTime dataValidade)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            DataValidade = dataValidade;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Nome, "nome");
            VerificaNulo(ref erros, Descricao, "descrição");
            VerificaNulo(ref erros, Lote, "lote");
            VerificaDataValidade(ref erros, DataValidade);
            return erros;
        }
    }
}
