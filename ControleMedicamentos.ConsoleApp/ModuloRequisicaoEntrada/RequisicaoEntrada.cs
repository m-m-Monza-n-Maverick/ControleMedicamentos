using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicaoEntrada
{
    internal class RequisicaoEntrada : EntidadeBase
    {
        public DateTime dataRequisicaoEntrada;

        public Medicamento medicamento;

        public Fornecedor fornecedor;

        public Funcionario funcionario;

        public int quantidadeRequisitada;
        public RequisicaoEntrada(DateTime dataRequisicaoEntrada, Medicamento medicamento,Fornecedor fornecedor, Funcionario funcionario, int quantidadeRequisitada)
        {
            this.dataRequisicaoEntrada = dataRequisicaoEntrada;
            this.medicamento = medicamento;
            this.fornecedor = fornecedor;
            this.funcionario = funcionario;
            this.quantidadeRequisitada = quantidadeRequisitada;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, medicamento, "medicamento");
            VerificaNulo(ref erros, fornecedor, "fornecedor");
            VerificaNulo(ref erros, funcionario, "funcionário");
            VerificaNulo(ref erros, quantidadeRequisitada.ToString(), "quantidade");
            return erros;
        }
    }
}
