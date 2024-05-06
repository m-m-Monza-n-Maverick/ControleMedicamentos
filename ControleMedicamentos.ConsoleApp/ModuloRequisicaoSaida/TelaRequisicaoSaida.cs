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

            Console.WriteLine( "{0, -5} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                "Id", "Medicamento", "Paciente", "Data de Requisição", "Quantidade" );

            EntidadeBase[] requisicoesCadastradas = repositorio.SelecionarTodos();

            foreach (RequisicaoSaida requisicao in requisicoesCadastradas)
            {
                if (requisicao == null) continue;

                Console.WriteLine(
                    "{0, -5} | {1, -15} | {2, -15} | {3, -20} | {4, -5}",
                    requisicao.Id,
                    requisicao.Medicamento.Nome,
                    requisicao.Paciente.Nome,
                    requisicao.DataRequisicao.ToString("d"),
                    requisicao.QuantidadeRetirada );
            }

            if (exibirTitulo) RecebeString("\n'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            int quantidade = 0, idEscolhido = 0;

            EntidadeBase medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            EntidadeBase pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(1);
            EntidadeBase novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);

            TabelaDeCadastro("{0, -5} | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome);
            RecebeAtributo(
                () => novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade),
                () => medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idEscolhido),
                ref novaRequisicaoSaida, ref medicamentoSelecionado, telaMedicamento, "medicamento", ref idEscolhido);
            //ExibirMensagem($"Medicamento selecionado: {medicamentoSelecionado.Nome}\n", ConsoleColor.Cyan);
            
            TabelaDeCadastro("{0, -5} | {1, -10}  | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome);
            RecebeAtributo(
                () => novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade),
                () => pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(idEscolhido),
                ref novaRequisicaoSaida, ref pacienteSelecionado, telaPaciente, "paciente", ref idEscolhido);
            //ExibirMensagem($"Propriedade selecionado: {pacienteSelecionado.Nome}\n", ConsoleColor.Cyan);

            TabelaDeCadastro("{0, -5} | {1, -10}  | {2, -10} | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome);
            RecebeAtributo(() => novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade), ref novaRequisicaoSaida, ref quantidade, "quantidade");
            return novaRequisicaoSaida;
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando requisição de saída...\n");
            Console.WriteLine("{0, -5} | {1, -10} | {2, -10} | {3, -5}", "Id", "Medicamento", "Paciente", "Quantidade");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2]);
        }
    }
}
