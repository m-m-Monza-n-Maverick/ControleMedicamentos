using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class TelaBase
    {
        public string tipoEntidade = "";
        public RepositorioBase repositorio;

        public void ApresentarMenu(ref bool sair)
        {
            bool retornar = true;
            while (retornar) 
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
                Console.WriteLine("R - Retornar");
                Console.WriteLine("S - Sair");

                string operacaoEscolhida = RecebeString("\nEscolha uma das opções: ");
                retornar = false;

                switch (operacaoEscolhida)
                {
                    case "1": Registrar(); break;
                    case "2": Editar(); break;
                    case "3": Excluir(); break;
                    case "4": VisualizarRegistros(true); break;
                    case "R": break;
                    case "S": sair = true; break;
                    default: OpcaoInvalida(ref retornar); break;
                }
            }
        }

        public virtual void Registrar()
        {
            ApresentarCabecalho();

            Console.WriteLine($"Cadastrando {tipoEntidade}...");

            Console.WriteLine();

            EntidadeBase entidade = ObterRegistro();

            ArrayList erros = entidade.Validar();

            if (erros.Count > 0)
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

            ArrayList erros = entidade.Validar();

            if (erros.Count > 0)
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

        #region Auxiliares
        protected void ApresentarErros(ArrayList erros)
        {
            foreach (string erro in erros) ExibirMensagem(erro, ConsoleColor.Red);
            Console.ReadKey(true);
        }
        protected void ApresentarCabecalho()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|       Controle de Medicamentos       |");
            Console.WriteLine("----------------------------------------\n");
        }
        public void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.Write("\n" + mensagem);
            Console.ResetColor();
        }

        protected abstract EntidadeBase ObterRegistro();
        public void OpcaoInvalida(ref bool retornar)
        {
            ExibirMensagem("        Opção inválida. Tente novamente ", ConsoleColor.Red);
            retornar = true;
            Console.ReadKey(true);
        }
        public void OpcaoInvalida()
        {
            ExibirMensagem("        Opção inválida. Tente novamente ", ConsoleColor.Red);
            Console.ReadKey(true);
        }
        public static string RecebeString(string texto)
        {
            Console.Write(texto);
            return Console.ReadLine().ToUpper();
        }
        #endregion
    }
}
