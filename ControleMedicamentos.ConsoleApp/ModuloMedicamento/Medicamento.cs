using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento : EntidadeBase
    {
        public Medicamento(string nome, string descricao, string lote, DateTime dataValidade)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            DataValidade = dataValidade;
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        private DateTime DataValidade { get; set; }
        public int Quantidade { get; set; } = 5;

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificarNulo(ref erros, Nome, "nome");
            VerificarNulo(ref erros, Descricao, "descrição");
            VerificarNulo(ref erros, Lote, "lote");
            VerificaData(ref erros, DataValidade);
            return erros;
        }
    }
}
