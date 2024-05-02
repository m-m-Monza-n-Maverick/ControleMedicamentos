﻿using System;
using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;

namespace ControleMedicamentos.ConsoleApp.ModuloFornecedor
{
    internal class TelaFornecedor : TelaBase
    {
        public TelaFornecedor(RepositorioBase repositorio, string nome)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (exibirTitulo)
            {
                ApresentarCabecalho();

                Console.WriteLine("Visualizando Fornecedores...");
            }

            Console.WriteLine();

            Console.WriteLine(
                "{0, -10} | {1, -20} | {2, -20}| {3, -20}",
                "Id", "Nome", "CNPJ", "Telefone"
            );

            EntidadeBase[] fornecedoresCadastrados = repositorio.SelecionarTodos();

            foreach (Fornecedor fornecedor in fornecedoresCadastrados)
            {
                if (fornecedor == null)
                    continue;

                Console.WriteLine(
                    "{0, -10} | {1, -20} | {2, -20} | {3, -20}",
                    fornecedor.Id, fornecedor.nome, fornecedor.cnpj, fornecedor.telefone
                );
            }

            Console.ReadKey(true);
            Console.WriteLine();
        }

        protected override EntidadeBase ObterRegistro()
        {
            Console.WriteLine("Digite o nome do fornecedor: ");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o telefone do fornecedor: ");
            string telefone = Console.ReadLine();

            Console.WriteLine("Digite o CNPJ do fornecedor: ");
            string cnpj = Console.ReadLine();

            Fornecedor fornecedor = new Fornecedor(nome, telefone, cnpj);

            return fornecedor;
        }
    }
}