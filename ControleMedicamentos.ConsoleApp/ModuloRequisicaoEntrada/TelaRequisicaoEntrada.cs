using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicaoEntrada
{
    internal class TelaRequisicaoEntrada : TelaBase
    {
        public TelaMedicamento telaMedicamento;
        public TelaFornecedor telaFornecedor;
        public TelaFuncionario telaFuncionario;

        public TelaRequisicaoEntrada(TelaMedicamento telaMedicamento, TelaFornecedor telaFornecedor, TelaFuncionario telaFuncionario, RepositorioBase repositorio, string nome)
        {
            this.telaMedicamento = telaMedicamento;
            this.telaFornecedor = telaFornecedor;
            this.telaFuncionario = telaFuncionario;
            this.repositorio = repositorio;
            tipoEntidade = nome;
        }

        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (exibirTitulo)
            {
                ApresentarCabecalho();

                Console.WriteLine("Visualizando Requisições de entrada...");
            }

            Console.WriteLine();

            Console.WriteLine(
                "{0, -10} | {1, -20} | {2, -20}| {3, -20}| {4, -20}| {5, -20}",
                "Id", "Data Requisição", "Medicamento", "Fornecedor", "Funcionário", "Quantidade"
            );

            EntidadeBase[] requisicoesEntradaCadastradas = repositorio.SelecionarTodos();

            foreach (RequisicaoEntrada requisicaoEntrada in requisicoesEntradaCadastradas)
            {
                if (requisicaoEntrada == null)
                    continue;

                Console.WriteLine(
                    "{0, -10} | {1, -20} | {2, -20} | {3, -20}| {4, -20}| {5, -20}",
                    requisicaoEntrada.Id, 
                    requisicaoEntrada.dataRequisicaoEntrada, 
                    requisicaoEntrada.medicamento.Nome,
                    requisicaoEntrada.fornecedor.nome, 
                    requisicaoEntrada.funcionario.nome, 
                    requisicaoEntrada.quantidadeRequisitada
                );
            }

            Console.ReadKey(true);
            Console.WriteLine();
        }

        protected override EntidadeBase ObterRegistro()
        {
            DateTime dataRequisicaoEntrada = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Blue;
            telaMedicamento.VisualizarRegistros(false);
            Console.ResetColor();

            Console.Write("Digite o ID do medicamento requisitado: ");
            int idMedicamento = Convert.ToInt32(Console.ReadLine());

            Medicamento medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idMedicamento);

            ExibirMensagem($"Medicamento selecionado: {medicamentoSelecionado.Nome}", ConsoleColor.Cyan);

            telaFornecedor.VisualizarRegistros(false);

            Console.Write("Digite o ID do fornecedor requisitado: ");
            int idFornecedor = Convert.ToInt32(Console.ReadLine());

            Fornecedor fornecedorSelecionado = (Fornecedor)telaFornecedor.repositorio.SelecionarPorId(idFornecedor);

            telaFuncionario.VisualizarRegistros(false);

            Console.Write("Digite o ID do Funcionario responsável: ");
            int idFuncionario = Convert.ToInt32(Console.ReadLine());

            Funcionario funcionarioSelecionado = (Funcionario)telaFuncionario.repositorio.SelecionarPorId(idFuncionario);

            Console.Write("\n\nDigite a quantidade requisitada do medicamento: ");
            int quantidade = Convert.ToInt32(Console.ReadLine());

            RequisicaoEntrada novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);

            return novaRequisicaoEntrada;
        }
    }
}
