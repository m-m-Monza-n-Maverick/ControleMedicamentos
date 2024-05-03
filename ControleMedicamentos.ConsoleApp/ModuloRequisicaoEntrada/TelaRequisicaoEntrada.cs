using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using System;
using System.Collections;
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

        public override void Registrar()
        {
            ApresentarCabecalhoEntidade($"Cadastrando {tipoEntidade}...\n");
            if (!telaMedicamento.repositorio.ExistemItensCadastrados() || !telaFornecedor.repositorio.ExistemItensCadastrados() || !telaFuncionario.repositorio.ExistemItensCadastrados()) RepositorioVazio();
            else
            {
                RequisicaoEntrada entidade = (RequisicaoEntrada)ObterRegistro();
                RealizaAcao(() => repositorio.Cadastrar(entidade), "cadastrado");
            }
        }
        public override void VisualizarRegistros(bool exibirTitulo)
        {
            if (!repositorio.ExistemItensCadastrados()) { RepositorioVazio(); return; }
            if (exibirTitulo) ApresentarCabecalhoEntidade("Visualizando requisições de entrada...\n");

            Console.WriteLine(
                "{0, -10} | {1, -20} | {2, -20}| {3, -20}| {4, -20}| {5, -20}",
                "Id", "Data Requisição", "Medicamento", "Fornecedor", "Funcionário", "Quantidade" );

            EntidadeBase[] requisicoesEntradaCadastradas = repositorio.SelecionarTodos();
            foreach (RequisicaoEntrada requisicaoEntrada in requisicoesEntradaCadastradas)
            {
                if (requisicaoEntrada == null) continue;

                Console.WriteLine(
                    "{0, -10} | {1, -20} | {2, -20} | {3, -20}| {4, -20}| {5, -20}",
                    requisicaoEntrada.Id, 
                    requisicaoEntrada.dataRequisicaoEntrada, 
                    requisicaoEntrada.medicamento.Nome,
                    requisicaoEntrada.fornecedor.nome, 
                    requisicaoEntrada.funcionario.nome, 
                    requisicaoEntrada.quantidadeRequisitada );
            }

            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
            else Console.WriteLine();
        }

        protected override EntidadeBase ObterRegistro()
        {
            Medicamento medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            Fornecedor fornecedorSelecionado = (Fornecedor)telaFornecedor.repositorio.SelecionarPorId(1);
            Funcionario funcionarioSelecionado = (Funcionario)telaFuncionario.repositorio.SelecionarPorId(1);
            DateTime dataRequisicaoEntrada = DateTime.Now;
            int quantidade = 0;
            ArrayList erros;
            EntidadeBase novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);

            RecebePropriedade(dataRequisicaoEntrada, ref medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade, ref novaRequisicaoEntrada, "Digite o ID do medicamento requisitado: ");
            ExibirMensagem($"Medicamento selecionado: {medicamentoSelecionado.Nome}\n", ConsoleColor.Cyan);

            RecebePropriedade(dataRequisicaoEntrada, medicamentoSelecionado, ref fornecedorSelecionado, funcionarioSelecionado, quantidade, ref novaRequisicaoEntrada, "\nDigite o ID do medicamento requisitado: ");
            ExibirMensagem($"Fornecedor selecionado: {fornecedorSelecionado.nome}\n", ConsoleColor.Cyan);

            RecebePropriedade(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, ref funcionarioSelecionado, quantidade, ref novaRequisicaoEntrada, "\nDigite o ID do medicamento requisitado: ");
            ExibirMensagem($"Funcionario selecionado: {funcionarioSelecionado.nome}\n", ConsoleColor.Cyan);

            RecebePropriedade(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, ref quantidade, ref novaRequisicaoEntrada, ref quantidade, "\nDigite o ID do medicamento requisitado: ");
            return novaRequisicaoEntrada;
        }

        private void RecebePropriedade(DateTime dataRequisicaoEntrada, ref Medicamento medicamentoSelecionado, Fornecedor fornecedorSelecionado, Funcionario funcionarioSelecionado, int quantidade, ref EntidadeBase novaRequisicaoEntrada, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Medicamentos...");
            telaMedicamento.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;
            do
            {
                int idMedicamento = RecebeInt(texto);
                medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idMedicamento);
                novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
                erros = novaRequisicaoEntrada.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(DateTime dataRequisicaoEntrada, Medicamento medicamentoSelecionado, ref Fornecedor fornecedorSelecionado, Funcionario funcionarioSelecionado, int quantidade, ref EntidadeBase novaRequisicaoEntrada, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nFornecedores...");
            telaFornecedor.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;
            do
            {
                int id = RecebeInt(texto);
                fornecedorSelecionado = (Fornecedor)telaFornecedor.repositorio.SelecionarPorId(id);
                novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
                erros = novaRequisicaoEntrada.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(DateTime dataRequisicaoEntrada, Medicamento medicamentoSelecionado, Fornecedor fornecedorSelecionado, ref Funcionario funcionarioSelecionado, int quantidade, ref EntidadeBase novaRequisicaoEntrada, string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nFuncionarios...");
            telaFuncionario.VisualizarRegistros(false);
            Console.ResetColor();
            ArrayList erros;
            do
            {
                int id = RecebeInt(texto);
                funcionarioSelecionado = (Funcionario)telaFuncionario.repositorio.SelecionarPorId(id);
                novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
                erros = novaRequisicaoEntrada.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
        private void RecebePropriedade(DateTime dataRequisicaoEntrada, Medicamento medicamentoSelecionado, Fornecedor fornecedorSelecionado, Funcionario funcionarioSelecionado, ref int quantidade, ref EntidadeBase novaRequisicaoEntrada, ref int propriedade, string texto)
        {
            ArrayList erros;
            do
            {
                propriedade = RecebeInt(texto);
                novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
                erros = novaRequisicaoEntrada.Validar();
                if (erros.Count != 0) ApresentarErros(erros.GetRange(0, 1));
            }
            while (erros.Count != 0);
        }
    }
}
