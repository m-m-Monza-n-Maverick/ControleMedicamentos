﻿using ControleMedicamentos.ConsoleApp.Compartilhado;
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
        public string nome { get; set; }
        public string cpf { get; set; }
        public string login { get; set; }
        public string senha { get; set; }

        public Funcionario(string nome, string cpf, string login, string senha)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.login = login;
            this.senha = senha;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificarNulo(ref erros, nome, "nome");
            VerificarNulo(ref erros, cpf, "CPF");
            VerificarNulo(ref erros, login, "login");
            VerificarNulo(ref erros, senha, "senha");
            return erros;
        }
    }
}
