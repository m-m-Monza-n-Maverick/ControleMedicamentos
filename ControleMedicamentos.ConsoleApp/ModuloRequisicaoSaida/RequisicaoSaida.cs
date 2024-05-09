using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class RequisicaoSaida : EntidadeBase
    {
        public EntidadeBase Medicamento { get; set; }
        public EntidadeBase Paciente { get; set; }
        public DateTime DataRequisicao { get; set; }
        public int QuantidadeRetirada { get; set; }

        public RequisicaoSaida(EntidadeBase medicamentoSelecionado, EntidadeBase pacienteSelecionado, int quantidade)
        {
            Medicamento = medicamentoSelecionado;
            Paciente = pacienteSelecionado;
            DataRequisicao = DateTime.Now;
            QuantidadeRetirada = quantidade;
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Medicamento);
            VerificaNulo(ref erros, Paciente);
            VerificaNulo(ref erros, QuantidadeRetirada.ToString(), "quantidade");
            return erros;
        }
        public bool RetirarMedicamento()
        {
            if (Medicamento.Quantidade < QuantidadeRetirada) return false;
            Medicamento.Quantidade -= QuantidadeRetirada;
            return true;
        }

        public override void AtualizarRegistro(EntidadeBase novoRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
