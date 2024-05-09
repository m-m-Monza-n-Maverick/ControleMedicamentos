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

            ArrayList requisicoesCadastradas = repositorio.SelecionarTodos();

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
            GeraOsAtributos(out int quantidade, out int idEscolhido, out EntidadeBase medicamentoSelecionado, out EntidadeBase pacienteSelecionado, out EntidadeBase novaRequisicaoSaida);
            GeraAsFuncoes(quantidade, idEscolhido, medicamentoSelecionado, pacienteSelecionado, novaRequisicaoSaida, out Action medicamento, out Action paciente, out Action novaRequisicao);

            TabelaDeCadastro("{0, -5} | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome);
            RecebeAtributo(novaRequisicao, medicamento, ref novaRequisicaoSaida, ref medicamentoSelecionado, telaMedicamento, "medicamento", ref idEscolhido);
            //ExibirMensagem($"Medicamento selecionado: {medicamentoSelecionado.Nome}\n", ConsoleColor.Cyan);

            TabelaDeCadastro("{0, -5} | {1, -10}  | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome);
            RecebeAtributo(novaRequisicao, paciente, ref novaRequisicaoSaida, ref pacienteSelecionado, telaPaciente, "paciente", ref idEscolhido);
            //ExibirMensagem($"Propriedade selecionado: {pacienteSelecionado.Nome}\n", ConsoleColor.Cyan);

            RecebeAtributo(() => novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade), ref novaRequisicaoSaida, ref quantidade,
                () => TabelaDeCadastro("{0, -5} | {1, -10}  | {2, -10} | ", medicamentoSelecionado.Nome, pacienteSelecionado.Nome));

            return novaRequisicaoSaida;
        }
        private void GeraOsAtributos(out int quantidade, out int idEscolhido, out EntidadeBase medicamentoSelecionado, out EntidadeBase pacienteSelecionado, out EntidadeBase novaRequisicaoSaida)
        {
            quantidade = 0;
            idEscolhido = 0;
            medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(1);
            novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
        }
        private void GeraAsFuncoes(int quantidade, int idEscolhido, EntidadeBase medicamentoSelecionado, EntidadeBase pacienteSelecionado, EntidadeBase novaRequisicaoSaida, out Action medicamento, out Action paciente, out Action novaRequisicao)
        {
            medicamento = () => medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idEscolhido);
            paciente = () => pacienteSelecionado = (Paciente)telaPaciente.repositorio.SelecionarPorId(idEscolhido);
            novaRequisicao = () => novaRequisicaoSaida = new RequisicaoSaida(medicamentoSelecionado, pacienteSelecionado, quantidade);
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
