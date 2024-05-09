using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
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
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando funcionários...\n");

            Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20}", 
                                "Id", "Nome", "CPF", "Login", "Senha");

            ArrayList funcionariosCadastrados = repositorio.SelecionarTodos();

            foreach (Funcionario funcionario in funcionariosCadastrados)
            {
                if (funcionario == null) continue;

                Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20}",
                    funcionario.Id, funcionario.Nome, funcionario.cpf, funcionario.login, funcionario.senha);
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            string nome = "-", cpf = "-", login = "-", senha = "-";
            EntidadeBase novoFuncionario = new Funcionario(nome, cpf, login, senha);

            RecebeAtributo(() => novoFuncionario = new Funcionario(nome, cpf, login, senha), ref novoFuncionario, ref nome,
                () => TabelaDeCadastro("{0, -5} | ", nome, cpf, login));

            RecebeAtributo(() => novoFuncionario = new Funcionario(nome, cpf, login, senha), ref novoFuncionario, ref cpf,
            () => TabelaDeCadastro("{0, -5} | {1, -15} | ", nome, cpf, login));

            RecebeAtributo(() => novoFuncionario = new Funcionario(nome, cpf, login, senha), ref novoFuncionario, ref login,
            () => TabelaDeCadastro("{0, -5} | {1, -15} | {2, -15} | ", nome, cpf, login));

            RecebeAtributo(() => novoFuncionario = new Funcionario(nome, cpf, login, senha), ref novoFuncionario, ref senha,
            () => TabelaDeCadastro("{0, -5} | {1, -15} | {2, -15} | {3, -15} | ", nome, cpf, login));

            return novoFuncionario;
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando novo funcionario...\n");
            Console.WriteLine("{0, -5} | {1, -15} | {2, -15} | {3, -15} | {4, -10}", "Id", "Nome", "CPF", "Login", "Senha");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2], texto[3]);
        }
    }
}
