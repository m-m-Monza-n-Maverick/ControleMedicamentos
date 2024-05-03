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
            ApresentarCabecalhoEntidade();

            if (telaMedicamento.repositorio.contadorId != 0 && telaPaciente.repositorio.contadorId != 0)
            {
                Console.WriteLine($"Cadastrando {tipoEntidade}...\n");
                RequisicaoSaida entidade = (RequisicaoSaida)ObterRegistro();

                bool conseguiuRetirar = entidade.RetirarMedicamento();

                if (!conseguiuRetirar)
                {
                    ExibirMensagem("A quantidade de retirada informada não está disponível.", ConsoleColor.DarkYellow);
                    return;
                }

                repositorio.Cadastrar(entidade);

                ExibirMensagem($"\nA {tipoEntidade} foi cadastrada com sucesso!", ConsoleColor.Green);
                Console.ReadKey(true);
            }
            else ExibirMensagem(" Ainda não existem medicamentos e/ou pacientes\n\t    'Enter' para retornar ", ConsoleColor.Red);
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (exibirTitulo)
            {
                ApresentarCabecalho();
                Console.WriteLine("Visualizando Requisições de Saída...");
            }

            Console.WriteLine( "\n{0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
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
                    requisicao.DataRequisicao.ToShortDateString(),
                    requisicao.QuantidadeRetirada );
            }
            Console.ReadKey(true);
        }

        protected override EntidadeBase ObterRegistro()
        {
            Medicamento medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            Paciente pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(1);
            int quantidade = 0;
            ArrayList erros;

            EntidadeBase novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);

            telaMedicamento.VisualizarRegistros(false);
            do
            {
                int idMedicamento = RecebeInt("Digite o ID do medicamento requisitado: ");
                medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idMedicamento);
                novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
                erros = novaRequisicaoSaida.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);

            telaPaciente.VisualizarRegistros(false);
            do
            {
                int idPaciente = RecebeInt("Digite o ID do paciente requisitado: ");
                pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(idPaciente);
                novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
                erros = novaRequisicaoSaida.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);

            RecebePropriedade(medicamentoSelecionado, pacienteSelecionado, ref quantidade, ref novaRequisicaoSaida, ref quantidade, "Digite a quantidade do medicamente que deseja retirar: ");

            return novaRequisicaoSaida;
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
