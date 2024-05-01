using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;

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
            ApresentarCabecalho();

            Console.WriteLine($"Cadastrando {tipoEntidade}...");

            Console.WriteLine();

            RequisicaoSaida entidade = (RequisicaoSaida)ObterRegistro();

            string[] erros = entidade.Validar();

            if (erros.Length > 0)
            {
                ApresentarErros(erros);
                return;
            }

            bool conseguiuRetirar = entidade.RetirarMedicamento();

            if (!conseguiuRetirar)
            {
                ExibirMensagem("A quantidade de retirada informada não está disponível.", ConsoleColor.DarkYellow);
                return;
            }

            repositorio.Cadastrar(entidade);

            ExibirMensagem($"O {tipoEntidade} foi cadastrado com sucesso!", ConsoleColor.Green);
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (exibirTitulo)
            {
                ApresentarCabecalho();

                Console.WriteLine("Visualizando Requisições de Saída...");
            }

            Console.WriteLine();

            Console.WriteLine(
                "{0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                "Id", "Medicamento", "Paciente", "Data de Requisição", "Quantidade"
            );

            EntidadeBase[] requisicoesCadastradas = repositorio.SelecionarTodos();

            foreach (RequisicaoSaida requisicao in requisicoesCadastradas)
            {
                if (requisicao == null)
                    continue;

                Console.WriteLine(
                    "{0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                    requisicao.Id,
                    requisicao.Medicamento.Nome,
                    requisicao.Paciente.Nome,
                    requisicao.DataRequisicao.ToShortDateString(),
                    requisicao.QuantidadeRetirada
                );
            }

            Console.ReadLine();
            Console.WriteLine();
        }

        protected override EntidadeBase ObterRegistro()
        {
            telaMedicamento.VisualizarRegistros(false);

            Console.Write("Digite o ID do medicamento requisitado: ");
            int idMedicamento = Convert.ToInt32(Console.ReadLine());

            Medicamento medicamentoSelecionado = (Medicamento)repositorio.SelecionarPorId(idMedicamento);

            telaPaciente.VisualizarRegistros(false);

            Console.Write("Digite o ID do paciente requisitante: ");
            int idPaciente = Convert.ToInt32(Console.ReadLine());

            Paciente pacienteSelecionado = (Paciente)repositorio.SelecionarPorId(idPaciente);

            Console.Write("Digite a quantidade do medicamente que deseja retirar: ");
            int quantidade = Convert.ToInt32(Console.ReadLine());

            RequisicaoSaida novaRequisicao = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);

            return novaRequisicao;
        }
    }
}
