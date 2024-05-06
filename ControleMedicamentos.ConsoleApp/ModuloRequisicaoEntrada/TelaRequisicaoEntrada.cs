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
                "{0, -5} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20}",
                "Id", "Data Requisição", "Medicamento", "Fornecedor", "Funcionário", "Quantidade" );

            EntidadeBase[] requisicoesEntradaCadastradas = repositorio.SelecionarTodos();
            foreach (RequisicaoEntrada requisicaoEntrada in requisicoesEntradaCadastradas)
            {
                if (requisicaoEntrada == null) continue;

                Console.WriteLine(
                    "{0, -5} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20}",
                    requisicaoEntrada.Id, 
                    requisicaoEntrada.dataRequisicaoEntrada.ToString("d"), 
                    requisicaoEntrada.medicamento.Nome,
                    requisicaoEntrada.fornecedor.Nome, 
                    requisicaoEntrada.funcionario.Nome, 
                    requisicaoEntrada.quantidadeRequisitada );
            }
            if (exibirTitulo) RecebeString("\n 'Enter' para continuar ");
        }
        protected override EntidadeBase ObterRegistro()
        {
            GeraOsAtributos(out int quantidade, out int idEscolhido, out DateTime dataRequisicaoEntrada, out EntidadeBase medicamentoSelecionado, out EntidadeBase fornecedorSelecionado, out EntidadeBase funcionarioSelecionado, out EntidadeBase novaRequisicaoEntrada);
            GeraAsFuncoes(quantidade, idEscolhido, dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, novaRequisicaoEntrada, out Action medicamento, out Action fornecedor, out Action funcionario, out Action novaRequisicao);

            TabelaDeCadastro("{0, -5} | ", medicamentoSelecionado.Nome, fornecedorSelecionado.Nome, funcionarioSelecionado.Nome);
            RecebeAtributo(novaRequisicao, medicamento, ref novaRequisicaoEntrada, ref medicamentoSelecionado, telaMedicamento, "medicamento", ref idEscolhido);

            TabelaDeCadastro("{0, -5} | {1, -20} | ", medicamentoSelecionado.Nome, fornecedorSelecionado.Nome, funcionarioSelecionado.Nome);
            RecebeAtributo(novaRequisicao, fornecedor, ref novaRequisicaoEntrada, ref fornecedorSelecionado, telaFornecedor, "fornecedor", ref idEscolhido);

            TabelaDeCadastro("{0, -5} | {1, -20} | {2, -20} | ", medicamentoSelecionado.Nome, fornecedorSelecionado.Nome, funcionarioSelecionado.Nome);
            RecebeAtributo(novaRequisicao, funcionario, ref novaRequisicaoEntrada, ref funcionarioSelecionado, telaFuncionario, "funcionario", ref idEscolhido);

            RecebeAtributo(() => novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade), ref novaRequisicaoEntrada, ref quantidade,
                () => TabelaDeCadastro("{0, -5} | {1, -20} | {2, -20} | {3, -20} | ", medicamentoSelecionado.Nome, fornecedorSelecionado.Nome, funcionarioSelecionado.Nome));

            return novaRequisicaoEntrada;
        }

        public void GeraOsAtributos(out int quantidade, out int idEscolhido, out DateTime dataRequisicaoEntrada, out EntidadeBase medicamentoSelecionado, out EntidadeBase fornecedorSelecionado, out EntidadeBase funcionarioSelecionado, out EntidadeBase novaRequisicaoEntrada)
        {
            quantidade = 0;
            idEscolhido = 0;
            dataRequisicaoEntrada = DateTime.Now;
            medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(1);
            fornecedorSelecionado = (Fornecedor)telaFornecedor.repositorio.SelecionarPorId(1);
            funcionarioSelecionado = (Funcionario)telaFuncionario.repositorio.SelecionarPorId(1);
            novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
        }
        private void GeraAsFuncoes(int quantidade, int idEscolhido, DateTime dataRequisicaoEntrada, EntidadeBase medicamentoSelecionado, EntidadeBase fornecedorSelecionado, EntidadeBase funcionarioSelecionado, EntidadeBase novaRequisicaoEntrada, out Action medicamento, out Action fornecedor, out Action funcionario, out Action novaRequisicao)
        {
            medicamento = () => medicamentoSelecionado = (Medicamento)telaMedicamento.repositorio.SelecionarPorId(idEscolhido);
            fornecedor = () => fornecedorSelecionado = (Fornecedor)telaFornecedor.repositorio.SelecionarPorId(idEscolhido);
            funcionario = () => funcionarioSelecionado = (Funcionario)telaFuncionario.repositorio.SelecionarPorId(idEscolhido);
            novaRequisicao = () => novaRequisicaoEntrada = new RequisicaoEntrada(dataRequisicaoEntrada, medicamentoSelecionado, fornecedorSelecionado, funcionarioSelecionado, quantidade);
        }
        protected override void TabelaDeCadastro(params string[] texto)
        {
            Console.Clear();
            ApresentarCabecalhoEntidade($"Cadastrando requisição de entrada...\n");
            Console.WriteLine("{0, -5} | {1, -20} | {2, -20} | {3, -20} | {4, -20}", "Id", "Medicamento", "Fornecedor", "Funcionário", "Quantidade");
            Console.Write(texto[0], repositorio.contadorId + 1, texto[1], texto[2], texto[3]);
        }
    }
}
