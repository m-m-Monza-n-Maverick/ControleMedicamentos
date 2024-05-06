using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class TelaPaciente : TelaBase
    {
        public TelaPaciente(RepositorioBase repositorio, string nome)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando pacientes...\n");

            Console.WriteLine("{0, -10} | {1, -15} | {2, -15} | {3, -15}",
                                 "Id", "Nome", "Telefone", "Cartão do SUS" );

            EntidadeBase[] pacientesCadastrados = repositorio.SelecionarTodos();

            foreach (Paciente paciente in pacientesCadastrados)
            {
                if (paciente == null) continue;

                Console.WriteLine(
                    "{0, -10} | {1, -15} | {2, -15} | {3, -15}",
                    paciente.Id, paciente.Nome, paciente.Telefone, paciente.CartaoSus );
            }

            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            string nome = "-", telefone = "-", cartaoSUS = "-";
            EntidadeBase novoPaciente = new Paciente(nome, telefone, cartaoSUS);

            TabelaDeCadastro ("{0, -10} | ", nome, telefone);
            RecebeAtributo(() => novoPaciente = new Paciente(nome, telefone, cartaoSUS), ref novoPaciente, ref nome);

            TabelaDeCadastro("{0, -10} | {1, -15} | ", nome, telefone);
            RecebeAtributo(() => novoPaciente = new Paciente(nome, telefone, cartaoSUS), ref novoPaciente, ref telefone);

            TabelaDeCadastro("{0, -10} | {1, -15} | {2, -15} | ", nome, telefone);
            RecebeAtributo(() => novoPaciente = new Paciente(nome, telefone, cartaoSUS), ref novoPaciente, ref cartaoSUS);

            return novoPaciente;
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando novo paciente...\n");
            Console.WriteLine("{0, -10} | {1, -15} | {2, -15} | {3, -15}", "Id", "Nome", "Telefone", "Cartão do SUS");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2]);
        }
    }
}
