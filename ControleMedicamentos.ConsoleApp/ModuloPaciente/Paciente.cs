using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class Paciente : EntidadeBase
    {
        public string Telefone { get; set; }
        public string CartaoSus { get; set; }

        public Paciente(string nome, string telefone, string cartaoSus)
        {
            Nome = nome;
            Telefone = telefone;
            CartaoSus = cartaoSus;
        }
        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Nome, "Nome");
            VerificaNulo(ref erros, Telefone, "telefone");
            VerificaNulo(ref erros, CartaoSus, "cartão do SUS");
            return erros;
        }

        public override void AtualizarRegistro(EntidadeBase novoRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
