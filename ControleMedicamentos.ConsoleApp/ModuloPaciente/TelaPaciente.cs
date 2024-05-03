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
            if (exibirTitulo)
            {
                ApresentarCabecalho();

                Console.WriteLine("Visualizando Pacientes...");
            }

            Console.WriteLine();

            Console.WriteLine(
                "{0, -10} | {1, -15} | {2, -15} | {3, -15}",
                "Id", "Nome", "Telefone", "Cartão do SUS"
            );

            EntidadeBase[] pacientesCadastrados = repositorio.SelecionarTodos();

            foreach (Paciente paciente in pacientesCadastrados)
            {
                if (paciente == null)
                    continue;

                Console.WriteLine(
                    "{0, -10} | {1, -15} | {2, -15} | {3, -15}",
                    paciente.Id, paciente.Nome, paciente.Telefone, paciente.CartaoSus
                );
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
            else Console.WriteLine();
        }

        protected override EntidadeBase ObterRegistro()
        {
            string nome = "a", telefone = "a", cartaoSUS = "a";
            EntidadeBase novoPaciente = new Paciente(nome, telefone, cartaoSUS);

            RecebePropriedade(ref nome, ref telefone, ref cartaoSUS, ref novoPaciente, ref nome, "Digite o nome: ");
            RecebePropriedade(ref nome, ref telefone, ref cartaoSUS, ref novoPaciente, ref telefone, "Digite o telefone: ");
            RecebePropriedade(ref nome, ref telefone, ref cartaoSUS, ref novoPaciente, ref cartaoSUS, "Digite o cartão SUS: ");

            return novoPaciente;
        }

        private void RecebePropriedade(ref string nome, ref string telefone, ref string cartaoSUS, ref EntidadeBase novoFuncionario, ref string propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeString(texto);
                novoFuncionario = new Paciente(nome, telefone, cartaoSUS);
                erros = novoFuncionario.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
    }
}
