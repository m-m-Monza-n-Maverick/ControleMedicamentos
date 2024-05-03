using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class RequisicaoSaida : EntidadeBase
    {
        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public DateTime DataRequisicao { get; set; }
        public int QuantidadeRetirada { get; set; }

        public RequisicaoSaida(Medicamento medicamentoSelecionado, Paciente pacienteSelecionado, int quantidade)
        {
            Medicamento = medicamentoSelecionado;
            Paciente = pacienteSelecionado;
            DataRequisicao = DateTime.Now;
            QuantidadeRetirada = quantidade;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Medicamento, "medicamento");
            VerificaNulo(ref erros, Paciente, "paciente");
            VerificaNulo(ref erros, QuantidadeRetirada.ToString(), "quantidade");
            return erros;
        }
        public bool RetirarMedicamento()
        {
            if (Medicamento.Quantidade < QuantidadeRetirada) return false;
            Medicamento.Quantidade -= QuantidadeRetirada;
            return true;
        }
    }
}
