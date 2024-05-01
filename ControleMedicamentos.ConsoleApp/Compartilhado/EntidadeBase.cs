using System.Collections;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class EntidadeBase
    {
        public int Id { get; set; }

        public abstract ArrayList Validar();
        protected void VerificarNulo(ref ArrayList erros, string campoTestado, string mostraCampo)
        {
            if (string.IsNullOrEmpty(campoTestado.Trim()))
                erros.Add($"O(a) \"{mostraCampo}\" precisa ser preenchido(a)");
        }
        protected void VerificaData(ref ArrayList erros, DateTime DataValidade)
        {
            DateTime hoje = DateTime.Now.Date;
            if (DataValidade < hoje)
                erros.Add("O campo \"data de validade\" não pode ser menor que a data atual");
        }
    }
}
