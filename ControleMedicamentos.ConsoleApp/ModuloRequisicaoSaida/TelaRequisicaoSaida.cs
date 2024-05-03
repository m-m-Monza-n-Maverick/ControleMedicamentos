using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using System.Collections;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class TelaRequisicaoSaida : TelaBase
    {
        public TelaPaciente telaPaciente;
        public TelaMedicamento telaMedicamento;
        public TelaRequisicaoSaida(TelaPaciente telaPaciente, TelaMedicamento telaMedicamento, RepositorioBase repositorio, string nome)
        {
            this.telaPaciente = telaPaciente;
            this.telaMedicamento = telaMedicamento;
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void Registrar()
        {
            ApresentarCabecalhoEntidade($"Cadastrando {tipoEntidade}...\n");
            if (!telaMedicamento.repositorio.ExistemItensCadastrados() || !telaPaciente.repositorio.ExistemItensCadastrados()) RepositorioVazio();
            else
            {
                RequisicaoSaida entidade = (RequisicaoSaida)ObterRegistro();

                if (!entidade.RetirarMedicamento())
                {
                    ExibirMensagem("A quantidade de retirada informada não está disponível.", ConsoleColor.DarkYellow);
                    Console.ReadKey(true);
                }
                else RealizaAcao(() => repositorio.Cadastrar(entidade), "cadastrado");
            }
        }
        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando Requisições de Saída...\n");

            Console.WriteLine( "{0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                "Id", "Medicamento", "Paciente", "Data de Requisição", "Quantidade" );

            EntidadeBase[] requisicoesCadastradas = repositorio.SelecionarTodos();

            foreach (RequisicaoSaida requisicao in requisicoesCadastradas)
            {
                if (requisicao == null) continue;

                Console.WriteLine(
                    "{0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                    requisicao.Id,
                    requisicao.Medicamento.Nome,
                    requisicao.Paciente.Nome,
                    requisicao.DataRequisicao.ToString("d"),
                    requisicao.QuantidadeRetirada );
            }

            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
            else Console.WriteLine();
        }
        protected override EntidadeBase ObterRegistro()
        {
            Medicamento medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            Paciente pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(1);
            int quantidade = 0;
            ArrayList erros;
            EntidadeBase novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);

            RecebePropriedade(ref medicamentoSelecionado, pacienteSelecionado, quantidade, ref novaRequisicaoSaida, "Digite o ID do medicamento requisitado: ");
            ExibirMensagem($"Medicamento selecionado: {medicamentoSelecionado.Nome}\n", ConsoleColor.Cyan);
            
            RecebePropriedade(medicamentoSelecionado, ref pacienteSelecionado, quantidade, ref novaRequisicaoSaida, "Digite o ID do paciente requisitante: ");
            ExibirMensagem($"Propriedade selecionado: {pacienteSelecionado.Nome}\n", ConsoleColor.Cyan);
            
            RecebePropriedade(medicamentoSelecionado, pacienteSelecionado, ref quantidade, ref novaRequisicaoSaida, ref quantidade, "Digite a quantidade do medicamente que deseja retirar: ");
            return novaRequisicaoSaida;
        }

        private void RecebePropriedade(ref Medicamento medicamentoSelecionado, Paciente pacienteSelecionado, int quantidade, ref EntidadeBase novaRequisicaoSaida, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Medicamentos...");
            telaMedicamento.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;
            do
            {
                int idMedicamento = RecebeInt(texto);
                medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idMedicamento); 
                novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
                erros = novaRequisicaoSaida.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(Medicamento medicamentoSelecionado, ref Paciente pacienteSelecionado, int quantidade, ref EntidadeBase novaRequisicaoSaida, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nPacientes...");
            telaPaciente.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;
            do
            {
                int idPaciente = RecebeInt(texto);
                pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(idPaciente);
                novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
                erros = novaRequisicaoSaida.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(Medicamento medicamentoSelecionado, Paciente pacienteSelecionado, ref int quantidade, ref EntidadeBase novaRequisicaoSaida, ref int propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeInt(texto);
                novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
                erros = novaRequisicaoSaida.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
    }
}
