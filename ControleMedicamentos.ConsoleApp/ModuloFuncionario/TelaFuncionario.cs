using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ControleMedicamentos.ConsoleApp.ModuloFuncionario
{
    internal class TelaFuncionario : TelaBase
    {
        public TelaFuncionario(RepositorioBase repositorio, string nome)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (exibirTitulo)
            {
                ApresentarCabecalho();
                Console.WriteLine("Visualizando funcionarios...");
            }

            Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20}", "Id", "Nome", "CPF", "Login", "Senha");

            EntidadeBase[] funcionariosCadastrados = repositorio.SelecionarTodos();

            foreach (Funcionario funcionario in funcionariosCadastrados)
            {
                if (funcionario == null) continue;

                Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20}",
                    funcionario.Id, funcionario.nome, funcionario.cpf, funcionario.login, funcionario.senha);
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }

        protected override EntidadeBase ObterRegistro()
        {
            string nome = "a", cpf = "a", login = "a", senha = "a";
            EntidadeBase novoFuncionario = new Funcionario(nome, cpf, login, senha);

            RecebePropriedade(ref nome, ref cpf, ref login, ref senha, ref novoFuncionario, ref nome, "Digite o nome: ");
            RecebePropriedade(ref nome, ref cpf, ref login, ref senha, ref novoFuncionario, ref cpf, "Digite o CPF: ");
            RecebePropriedade(ref nome, ref cpf, ref login, ref senha, ref novoFuncionario, ref login, "Digite o login: ");
            RecebePropriedade(ref nome, ref cpf, ref login, ref senha, ref novoFuncionario, ref senha, "Digite a senha: ");

            return new Funcionario(nome, cpf, login, senha);
        }

        private void RecebePropriedade(ref string nome, ref string cpf, ref string login, ref string senha, ref EntidadeBase novoFuncionario, ref string propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeString(texto);
                novoFuncionario = new Funcionario(nome, cpf, login, senha);
                erros = novoFuncionario.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
    }
}
