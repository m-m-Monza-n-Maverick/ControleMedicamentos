using ControleMedicamentos.ConsoleApp.Compartilhado;
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
            if (exibirTitulo)
            {
                ApresentarCabecalhoEntidade();
                Console.WriteLine("Visualizando medicamentos...");
            }

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

            RecebePropriedade(ref nome, ref descricao, ref lote, dataValidade, ref novoMedicamento, ref nome, "Digite o nome: ");
            RecebePropriedade(ref nome, ref descricao, ref lote, dataValidade, ref novoMedicamento, ref descricao, "Digite a descrição: ");
            RecebePropriedade(ref nome, ref descricao, ref lote, dataValidade, ref novoMedicamento, ref lote, "Digite o lote: ");
            RecebePropriedade(nome, descricao, lote, ref dataValidade, ref novoMedicamento, ref dataValidade, "Digite a data de validade: ");
            
            return novoMedicamento;
        }

        private void RecebePropriedade(ref string nome, ref string descricao, ref string lote, DateTime dataValidade, ref EntidadeBase novoMedicamento, ref string propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeString(texto);
                novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade);
                erros = novoMedicamento.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(string nome, string descricao, string lote, ref DateTime dataValidade, ref EntidadeBase novoMedicamento, ref DateTime propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                dataValidade = RecebeData("Digite a data de validade: ");
                novoMedicamento = new Medicamento(nome, descricao, lote, dataValidade);
                erros = novoMedicamento.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
    }
}
