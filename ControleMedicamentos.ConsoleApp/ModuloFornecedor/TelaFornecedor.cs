using System;
using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using System.Collections;

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
                    fornecedor.Id, fornecedor.nome, fornecedor.cnpj, fornecedor.telefone
                );
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }

        protected override EntidadeBase ObterRegistro()
        {
            string nome = "a", telefone = "a", cnpj = "a";
            EntidadeBase novoFornecedor = new Fornecedor(nome, telefone, cnpj);

            RecebePropriedade(ref nome, ref telefone, ref cnpj, ref novoFornecedor, ref nome, "Digite o nome: ");
            RecebePropriedade(ref nome, ref telefone, ref cnpj, ref novoFornecedor, ref telefone, "Digite o telefone: ");
            RecebePropriedade(ref nome, ref telefone, ref cnpj, ref novoFornecedor, ref cnpj, "Digite o CNPJ: ");

            return novoFornecedor;
        }

        private void RecebePropriedade(ref string nome, ref string telefone, ref string cnpj, ref EntidadeBase novoMedicamento, ref string propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeString(texto);
                novoMedicamento = new Fornecedor(nome, telefone, cnpj);
                erros = novoMedicamento.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }

    }
}
