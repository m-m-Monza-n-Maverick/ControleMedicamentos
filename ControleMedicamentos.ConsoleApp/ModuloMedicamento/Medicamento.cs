using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento : EntidadeBase
    {
        public string Descricao { get; set; }
        public string Lote { get; set; }
        protected DateTime DataValidade { get; set; }
        protected EntidadeBase Fornecedor { get; set; }

        public Medicamento(string nome, string descricao, string lote, DateTime dataValidade)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            DataValidade = dataValidade;
        }
        public override void AtualizarRegistro(EntidadeBase novoRegistro)
        {
            Medicamento novasInformacoes = (Medicamento)novoRegistro;

            this.Nome = novasInformacoes.Nome;
            this.Descricao = novasInformacoes.Descricao;
            this.Lote = novasInformacoes.Lote;
            this.DataValidade = novasInformacoes.DataValidade;
            this.Fornecedor = novasInformacoes.Fornecedor;
            this.Quantidade = novasInformacoes.Quantidade;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Nome, "Nome");
            VerificaNulo(ref erros, Descricao, "descrição");
            VerificaNulo(ref erros, Lote, "lote");
            VerificaDataValidade(ref erros, DataValidade);
            return erros;
        }
    }
}
