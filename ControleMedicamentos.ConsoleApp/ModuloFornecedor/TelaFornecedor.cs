using System;
using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;

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
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando fornecedores...");

            Console.WriteLine(
                "{0, -10} | {1, -20} | {2, -20}| {3, -20}",
                "Id", "Nome", "CNPJ", "Telefone" );

            EntidadeBase[] fornecedoresCadastrados = repositorio.SelecionarTodos();

            foreach (Fornecedor fornecedor in fornecedoresCadastrados)
            {
                if (fornecedor == null)
                    continue;

                Console.WriteLine(
                    "{0, -10} | {1, -20} | {2, -20} | {3, -20}",
                    fornecedor.Id, fornecedor.Nome, fornecedor.cnpj, fornecedor.telefone
                );
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            string nome = "-", telefone = "-", cnpj = "-";
            EntidadeBase novoFornecedor = new Fornecedor(nome, telefone, cnpj);
            
            RecebeAtributo(() => novoFornecedor = new Fornecedor(nome, telefone, cnpj), ref novoFornecedor, ref nome,
                () => TabelaDeCadastro("{0, -10} | ", nome, telefone));

            RecebeAtributo(() => novoFornecedor = new Fornecedor(nome, telefone, cnpj), ref novoFornecedor, ref telefone,
                () => TabelaDeCadastro("{0, -10} | {1, -20} | ", nome, telefone));

            RecebeAtributo(() => novoFornecedor = new Fornecedor(nome, telefone, cnpj), ref novoFornecedor, ref cnpj,
                () => TabelaDeCadastro("{0, -10} | {1, -20} | {2, -20}| ", nome, telefone));

            return novoFornecedor;
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando novo fornecedor...\n");
            Console.WriteLine("{0, -10} | {1, -20} | {2, -20}| {3, -20}", "Id", "Nome", "Telefone", "CNPJ");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2]);
        }
    }
}
