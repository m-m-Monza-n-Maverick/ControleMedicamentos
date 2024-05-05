using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using System.Collections;

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
            else Console.WriteLine();
        }
        protected override EntidadeBase ObterRegistro()
        {
            string nome = "a", descricao = "a", lote = "a";
            DateTime dataValidade = DateTime.Now;

            EntidadeBase novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade);

            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref nome, "Nome");
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref descricao, "descrição");
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref lote, "lote");
            RecebeAtributo(() => novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade), ref novoMedicamento, ref dataValidade, "data de validade");

            return novoMedicamento;
        }
    }
}
