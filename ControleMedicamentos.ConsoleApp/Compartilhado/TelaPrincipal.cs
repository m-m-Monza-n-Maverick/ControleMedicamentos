using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal class TelaPrincipal : TelaBase
    {
        static TelaPaciente telaPaciente = new TelaPaciente(new RepositorioPaciente(), "paciente");
        static TelaMedicamento telaMedicamento = new TelaMedicamento(new RepositorioMedicamento(), "medicamento");
        static TelaFuncionario telaFuncionario = new TelaFuncionario(new RepositorioFuncionario(), "funcionário");
        TelaFornecedor telaFornecedor = new TelaFornecedor(new RepositorioFornecedor(), "fornecedor");
        TelaRequisicaoSaida telaRequisicaoSaida = new TelaRequisicaoSaida(telaPaciente, telaMedicamento, new RepositorioRequisicaoSaida(), "requisição");
        public void MenuPrincipal(ref bool sair)
        {
            Console.Clear();

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|       Controle de Medicamentos       |");
            Console.WriteLine("----------------------------------------\n");

            Console.WriteLine("1 - Cadastro de Pacientes");
            Console.WriteLine("2 - Cadastro de Medicamentos");
            Console.WriteLine("3 - Cadastro de Requisições de Saída");
            Console.WriteLine("4 - Cadastro de Funcionário");
            Console.WriteLine("5 - Cadastro de Fornecedor");
            Console.WriteLine("S - Sair");

            string opcaoEscolhida = RecebeString("\nEscolha uma das opções: ");

            switch (opcaoEscolhida)
            {
                case "1": telaPaciente.ApresentarMenu(ref sair); break;
                case "2": telaMedicamento.ApresentarMenu(ref sair); break;
                case "3": telaRequisicaoSaida.ApresentarMenu(ref sair); break;
                case "4": telaFuncionario.ApresentarMenu(ref sair); break;
                case "5": telaFornecedor.ApresentarMenu(ref sair); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(); break;
            }
        }
        public override void VisualizarRegistros(bool exibirTitulo) => throw new NotImplementedException();
        protected override EntidadeBase ObterRegistro() => throw new NotImplementedException();
    }
}
