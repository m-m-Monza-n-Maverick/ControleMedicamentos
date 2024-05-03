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
                ApresentarCabecalhoEntidade();

                Console.WriteLine($"\n1 - Cadastrar {tipoEntidade}");
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
            ApresentarCabecalhoEntidade();
            Console.WriteLine($"Cadastrando {tipoEntidade}...\n");

            EntidadeBase entidade = ObterRegistro();
            repositorio.Cadastrar(entidade);

            ExibirMensagem($"\nO(a) {tipoEntidade} foi cadastrado(a) com sucesso! ", ConsoleColor.Green);
            Console.ReadKey(true);
        }
        public void Editar()
        {
            ApresentarCabecalhoEntidade();
            Console.WriteLine($"Editando {tipoEntidade}...\n");

            VisualizarRegistros(false);

            int idEntidadeEscolhida = RecebeInt($"\nDigite o ID do {tipoEntidade} que deseja editar: ");

            if (!repositorio.Existe(idEntidadeEscolhida))
            {
                ExibirMensagem($"O {tipoEntidade} mencionado não existe!", ConsoleColor.DarkYellow);
                return;
            }

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

            ExibirMensagem($"\nO {tipoEntidade} foi editado com sucesso!", ConsoleColor.Green);
        }
        public void Excluir()
        {
            ApresentarCabecalhoEntidade();

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
            Console.WriteLine();
        }
        protected void ApresentarCabecalho()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|       Controle de Medicamentos       |");
            Console.WriteLine("----------------------------------------");
        }
        public void ApresentarCabecalhoEntidade()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"          Gestão de {tipoEntidade}s        ");
            Console.WriteLine("----------------------------------------");
        }
        public void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.Write(mensagem);
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
        public int RecebeInt(string texto)
        {
            Console.Write(texto);
            string quantidade = "", input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) NaoEhNumero(ref input, texto);

            foreach (char c in input.ToCharArray()) 
                if (Convert.ToInt32(c) >= 48 && Convert.ToInt32(c) <= 57) quantidade += c;
            
            if (quantidade.Length != input.Length) NaoEhNumero(ref quantidade, texto);

            return Convert.ToInt32(quantidade);
        }
        public DateTime RecebeData(string texto)
        {
            string data = RecebeString(texto);
            char[] dataValidade = data.ToCharArray();
            if (ValidaTabulacao(dataValidade) || ValidaDias(dataValidade) || ValidaMeses(dataValidade))
            {
                ExibirMensagem("\nData inválida! Tente novamente\n", ConsoleColor.Red);
                data = Convert.ToString(RecebeData(texto));
            }
            return Convert.ToDateTime(data);
        }

        #region Valida data
        public bool ValidaTabulacao(char[] dataValidade) => dataValidade.Length != 10 || dataValidade[2] != '/' || dataValidade[5] != '/';
        public bool ValidaDias(char[] dataValidade) => (dataValidade[0] != '0' && dataValidade[0] != '1' && dataValidade[0] != '2' && dataValidade[0] != '3') || (dataValidade[0] == '3' && dataValidade[1] != '0');
        public bool ValidaMeses(char[] dataValidade) => (dataValidade[3] != '0' && dataValidade[3] != '1') || (dataValidade[3] == '1' && dataValidade[4] != '0' && dataValidade[4] != '1' && dataValidade[4] != '2');
        #endregion

        #region Valida int
        public void NaoEhNumero(ref string input, string texto)
        {
            ExibirMensagem("Não é um número!\n", ConsoleColor.Red);
            input = Convert.ToString(RecebeInt(texto)); //Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "input" original (nula)
        }
        #endregion

        #endregion
    }
}
