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

        public EntidadeBase medicamento, fornecedor, funcionario;

        public int quantidadeRequisitada;
        public RequisicaoEntrada(DateTime dataRequisicaoEntrada, EntidadeBase medicamento,EntidadeBase fornecedor, EntidadeBase funcionario, int quantidadeRequisitada)
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
            VerificaNulo(ref erros, medicamento);
            VerificaNulo(ref erros, fornecedor);
            VerificaNulo(ref erros, funcionario);
            VerificaNulo(ref erros, quantidadeRequisitada.ToString(), "quantidade");
            return erros;
        }
    }
}
