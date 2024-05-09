using ControleMedicamentos.ConsoleApp.Compartilhado;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ControleMedicamentos.ConsoleApp.ModuloFuncionario
{
    internal class Funcionario : EntidadeBase
    {
        public string cpf { get; set; }
        public string login { get; set; }
        public string senha { get; set; }

        public Funcionario(string nome, string cpf, string login, string senha)
        {
            this.Nome = nome;
            this.cpf = cpf;
            this.login = login;
            this.senha = senha;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Nome, "Nome");
            VerificaNulo(ref erros, cpf, "CPF");
            VerificaNulo(ref erros, login, "login");
            VerificaNulo(ref erros, senha, "senha");
            return erros;
        }

        public override void AtualizarRegistro(EntidadeBase novoRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
