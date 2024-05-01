using ControleMedicamentos.ConsoleApp.Compartilhado;
using System.Collections;
namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class Paciente : EntidadeBase
    {
        public string Nome { get; set; }
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
            VerificarNulo(ref erros, Nome, "nome");
            VerificarNulo(ref erros, Telefone, "telefone");
            VerificarNulo(ref erros, CartaoSus, "cartão SUS");
            return erros;
        }
    }
}
