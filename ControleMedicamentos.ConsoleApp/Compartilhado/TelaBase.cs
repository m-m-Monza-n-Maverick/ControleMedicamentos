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
                ApresentarCabecalhoEntidade("");

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
            EntidadeBase entidade = ObterRegistro();
            RealizaAcao(() => repositorio.Cadastrar(entidade), "cadastrado");
        }
        protected abstract EntidadeBase ObterRegistro();
        public void Editar()
        {
            while (true)
            {
                if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }

                ApresentarCabecalhoEntidade($"\nEditando {tipoEntidade}...\n");
                VisualizarRegistros(false);

                int idEntidadeEscolhida = RecebeInt($"Digite o ID do {tipoEntidade} que deseja editar: ");

                if (!repositorio.Existe(idEntidadeEscolhida)) IdInvalido();
                else
                {
                    EntidadeBase entidade = ObterRegistro();
                    RealizaAcao(() => repositorio.Editar(idEntidadeEscolhida, entidade), "editado");
                    break;
                }
            }
        }
        public void Excluir()
        {
            while(true)
            {
                ApresentarCabecalhoEntidade($"\nExcluindo {tipoEntidade}...\n");
                VisualizarRegistros(false);

                int idRegistroEscolhido = RecebeInt($"Digite o ID do {tipoEntidade} que deseja excluir: ");

                if (!repositorio.Existe(idRegistroEscolhido)) IdInvalido();
                else
                {
                    RealizaAcao(() => repositorio.Excluir(idRegistroEscolhido), "excluído");
                    break;
                }
            }
        }
        public abstract void VisualizarRegistros(bool exibirTitulo);

        #region Auxiliares
        protected void RealizaAcao(Action acao, string acaoRealizada)
        {
            acao();
            ExibirMensagem($"\nO(a) {tipoEntidade} foi {acaoRealizada}(a) com sucesso!", ConsoleColor.Green);
            Console.ReadKey(true);
        }
        protected void ApresentarCabecalho()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("|       Controle de Medicamentos       |");
            Console.WriteLine("----------------------------------------");
        }
        public void ApresentarCabecalhoEntidade(string texto)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"          Gestão de {tipoEntidade}s        ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(texto);
        }
        protected void ApresentarErros(ArrayList erros)
        {
            foreach (string erro in erros) ExibirMensagem(erro, ConsoleColor.Red);
        }
        protected virtual void TabelaDeCadastro(params string[] texto) { }
        public void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.Write(mensagem);
            Console.ResetColor();
        }

        #region Inputs
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
        public void RecebeAtributo(Action funcao, ref EntidadeBase novaEntidade, ref string atributo)
        {
            ArrayList erros;
            do
            {
                atributo = Console.ReadLine();
                funcao();
                erros = novaEntidade.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        public void RecebeAtributo(Action funcao, ref EntidadeBase novaEntidade, ref int atributo, string texto)
        {
            ArrayList erros;
            do
            {
                atributo = RecebeInt("");
                funcao();
                erros = novaEntidade.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        public void RecebeAtributo(Action funcao, ref EntidadeBase novaEntidade, ref DateTime atributo, string texto)
        {
            ArrayList erros;
            do
            {
                atributo = RecebeData("");
                funcao();
                erros = novaEntidade.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        public void RecebeAtributo(Action funcao, Action atributo, ref EntidadeBase novaEntidade, ref EntidadeBase novoAtributo, TelaBase tela, string texto, ref int idEscolhido)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n\n{texto}s...");
            tela.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;

            do
            {
                idEscolhido = RecebeInt($"\nDigite o ID do {texto} requisitado: ");
                atributo();
                funcao();
                erros = novaEntidade.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }

        #endregion

        #region Validações
        public void OpcaoInvalida(ref bool retornar)
        {
            ExibirMensagem("Opção inválida. Tente novamente ", ConsoleColor.Red);
            retornar = true;
            Console.ReadKey(true);
        }
        public void OpcaoInvalida()
        {
            ExibirMensagem("Opção inválida. Tente novamente ", ConsoleColor.Red);
            Console.ReadKey(true);
        }
        public bool ValidaTabulacao(char[] dataValidade) => dataValidade.Length != 10 || dataValidade[2] != '/' || dataValidade[5] != '/';
        public bool ValidaDias(char[] dataValidade) => (dataValidade[0] != '0' && dataValidade[0] != '1' && dataValidade[0] != '2' && dataValidade[0] != '3') || (dataValidade[0] == '3' && dataValidade[1] != '0');
        public bool ValidaMeses(char[] dataValidade) => (dataValidade[3] != '0' && dataValidade[3] != '1') || (dataValidade[3] == '1' && dataValidade[4] != '0' && dataValidade[4] != '1' && dataValidade[4] != '2');
        public void NaoEhNumero(ref string input, string texto)
        {
            ExibirMensagem("Por favor, insira um número ", ConsoleColor.Red);
            input = Convert.ToString(RecebeInt(texto)); //Para garantir que, ao sair do loop, o método "RecebeInt" não vai puxar a "input" original (nula)
        }
        private void IdInvalido()
        {
            ExibirMensagem($"\nO {tipoEntidade} mencionado não existe! ", ConsoleColor.DarkYellow);
            Console.ReadKey(true);
        }
        protected void RepositorioVazio()
        {
            ExibirMensagem($"Ainda não existem itens cadastrados! ", ConsoleColor.Red);
            Console.ReadKey(true);
        }
        #endregion

        #endregion
    }
}
