﻿using System.Collections;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class EntidadeBase
    {
        public int Id { get; set; }

        public abstract ArrayList Validar();

        protected void VerificaNulo(ref ArrayList erros, string campoTestado, string mostraCampo)
        {
            if (string.IsNullOrEmpty(campoTestado))
                erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente");
        }
        protected void VerificaNulo(ref ArrayList erros, EntidadeBase campoTestado, string mostraCampo)
        {
            if (campoTestado == null)
                erros.Add($"\nEste ID não existe. Tente novamente");
        }
        protected void VerificaTamanho(ref ArrayList erros, string campoTestado, string mostraCampo, int tamanho)
        {
            if (string.IsNullOrEmpty(campoTestado))
                erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente");

            else if (campoTestado.Trim().Length < tamanho)
                erros.Add($"O(a) \"{mostraCampo}\" precisa ter mais do que {tamanho} caracteres");
        }
        protected void VerificaDataValidade(ref ArrayList erros, DateTime DataValidade)
        {
            DateTime hoje = DateTime.Now.Date;
            if (DataValidade < hoje)
                erros.Add("\nO campo \"data de validade\" não pode ser menor que a data atual. Tente novamente");
            else if (DataValidade.Year > 2100)
                erros.Add($"\n{DataValidade.Year}? Também não despiroque né. Tente novamente");
        }
    }
}
