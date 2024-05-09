using ControleMedicamentos.ConsoleApp.Compartilhado;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloFornecedor
{
    internal class Fornecedor : EntidadeBase
    {
        public string telefone;
        public string cnpj;

        public Fornecedor(string nome, string telefone, string cnpj)
        {
            Nome = nome;
            this.telefone = telefone;
            this.cnpj = cnpj;
        }

        public override void AtualizarRegistro(EntidadeBase novoRegistro)
        {
            throw new NotImplementedException();
        }

        public override ArrayList Validar()
        {
            ArrayList erros = new ArrayList();
            VerificaNulo(ref erros, Nome, "Nome");
            VerificaNulo(ref erros, telefone, "telefone");
            VerificaNulo(ref erros, cnpj, "CNPJ");
            return erros;
        }
    }
}
