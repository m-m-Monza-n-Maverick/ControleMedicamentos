using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class TelaMedicamento : TelaBase
    {
        public TelaMedicamento(RepositorioBase repositorio, string nome)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando medicamentos...\n");

            Console.WriteLine("{0, -10} | {1, -20} | {2, -20}", "Id", "Nome", "Quantidade");

            EntidadeBase[] medicamentosCadastrados = repositorio.SelecionarTodos();

            foreach (Medicamento medicamento in medicamentosCadastrados)
            {
                if (medicamento == null) continue;
                Console.WriteLine( "{0, -10} | {1, -20} | {2, -20}", medicamento.Id, medicamento.Nome, medicamento.Quantidade);
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            string nome = "-", descricao = "-", lote = "-";
            DateTime dataValidade = DateTime.Now;
            EntidadeBase novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade);
            
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref nome, 
                () => TabelaDeCadastro("{0, -5} | ", nome, descricao, lote));
            
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref descricao,
                () => TabelaDeCadastro("{0, -5} | {1, -10} | ", nome, descricao, lote));

            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref lote,
                () => TabelaDeCadastro("{0, -5} | {1, -10} | {2, -15} | ", nome, descricao, lote));
            
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref dataValidade,
                () => TabelaDeCadastro("{0, -5} | {1, -10} | {2, -15} | {3, -5} | ", nome, descricao, lote));

            return novoMedicamento;
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando novo medicamento...\n");
            Console.WriteLine("{0, -5} | {1, -10} | {2, -15} | {3, -5} | {4, -5}", "Id", "Nome", "Descrição", "Lote", "Data de validade");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2], texto[3]);
        }
    }
}
