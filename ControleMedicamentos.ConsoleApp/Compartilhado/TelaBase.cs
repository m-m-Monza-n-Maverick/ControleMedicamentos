using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class TelaBase
    {
        public string tipoEntidade = "";
        public RepositorioBase repositorio;

        public void ApresentarMenu(ref bool sair)
        {
            Console.Clear();

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"        Gestão de {tipoEntidade}s        ");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine();

            Console.WriteLine($"1 - Cadastrar {tipoEntidade}");
            Console.WriteLine($"2 - Editar {tipoEntidade}");
            Console.WriteLine($"3 - Excluir {tipoEntidade}");
            Console.WriteLine($"4 - Visualizar {tipoEntidade}s");

            Console.WriteLine("S - Voltar");

            Console.Write("\nEscolha uma das opções: ");
            string operacaoEscolhida = Console.ReadLine().ToUpper();

            switch (operacaoEscolhida)
            {
                case "1": Registrar(); break;
                case "2": Editar(); break;
                case "3": Excluir(); break;
                case "4": VisualizarRegistros(true); break;
                case "S": sair = true; break;
                default: OpcaoInvalida(); break;
            }
        }

        public virtual void Registrar()
        {
            ApresentarCabecalho();

            Console.WriteLine($"Cadastrando {tipoEntidade}...");

            Console.WriteLine();

            EntidadeBase entidade = ObterRegistro();

            string[] erros = entidade.Validar();

            if (erros.Length > 0)
            {
                ApresentarErros(erros);
                return;
            }

            repositorio.Cadastrar(entidade);

            ExibirMensagem($"O {tipoEntidade} foi cadastrado com sucesso!", ConsoleColor.Green);
        }

        public void Editar()
        {
            ApresentarCabecalho();

            Console.WriteLine($"Editando {tipoEntidade}...");

            Console.WriteLine();

            VisualizarRegistros(false);

            Console.Write($"Digite o ID do {tipoEntidade} que deseja editar: ");
            int idEntidadeEscolhida = Convert.ToInt32(Console.ReadLine());

            if (!repositorio.Existe(idEntidadeEscolhida))
            {
                ExibirMensagem($"O {tipoEntidade} mencionado não existe!", ConsoleColor.DarkYellow);
                return;
            }

            Console.WriteLine();

            EntidadeBase entidade = ObterRegistro();

            string[] erros = entidade.Validar();

            if (erros.Length > 0)
            {
                ApresentarErros(erros);
                return;
            }

            bool conseguiuEditar = repositorio.Editar(idEntidadeEscolhida, entidade);

            if (!conseguiuEditar)
            {
                ExibirMensagem($"Houve um erro durante a edição de {tipoEntidade}", ConsoleColor.Red);
                return;
            }

            ExibirMensagem($"O {tipoEntidade} foi editado com sucesso!", ConsoleColor.Green);
        }

        public void Excluir()
        {
            ApresentarCabecalho();

            Console.WriteLine($"Excluindo {tipoEntidade}...");

            Console.WriteLine();

            VisualizarRegistros(false);

            Console.Write($"Digite o ID do {tipoEntidade} que deseja excluir: ");
            int idRegistroEscolhido = Convert.ToInt32(Console.ReadLine());

            if (!repositorio.Existe(idRegistroEscolhido))
            {
                ExibirMensagem($"O {tipoEntidade} mencionado não existe!", ConsoleColor.DarkYellow);
                return;
            }

            bool conseguiuExcluir = repositorio.Excluir(idRegistroEscolhido);

            if (!conseguiuExcluir)
            {
                ExibirMensagem($"Houve um erro durante a exclusão do {tipoEntidade}", ConsoleColor.Red);
                return;
            }

            ExibirMensagem($"O {tipoEntidade} foi excluído com sucesso!", ConsoleColor.Green);
        }

        public abstract void VisualizarRegistros(bool exibirTitulo);

        protected void ApresentarErros(string[] erros)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < erros.Length; i++)
                Console.WriteLine(erros[i]);

            Console.ResetColor();
            Console.ReadLine();
        }

        protected void ApresentarCabecalho()
        {
            Console.Clear();

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|       Controle de Medicamentos       |");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine();
        }

        public void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;

            Console.WriteLine();

            Console.WriteLine(mensagem);

            Console.ResetColor();

            Console.ReadLine();
        }

        protected abstract EntidadeBase ObterRegistro();
        public void OpcaoInvalida()
        {
            Notificação(ConsoleColor.Red, "\n        Opção inválida. Tente novamente ");
            Console.ReadKey(true);
        }
        public void Notificação(ConsoleColor cor, string texto)
        {
            Console.ForegroundColor = cor;
            Console.Write(texto);
            Console.ResetColor();
        }

    }
}
