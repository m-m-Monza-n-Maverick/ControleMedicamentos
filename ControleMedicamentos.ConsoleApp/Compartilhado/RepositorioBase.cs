using System.Collections;
namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class RepositorioBase
    {
        protected ArrayList registros = new ArrayList();
        public int contadorId = 0;

        public void Cadastrar(EntidadeBase novoRegistro)
        {
            contadorId++;
            novoRegistro.Id = contadorId;
            registros.Add(novoRegistro);
        }

        public void Editar(int id, EntidadeBase novaEntidade)
        {
            novaEntidade.Id = id;
            registros[id - 1] = novaEntidade;
        }

        public void Excluir(int id) => registros[id - 1] = null;

        public bool ExistemItensCadastrados() => contadorId != 0;
        public ArrayList SelecionarTodos() => registros;
        public EntidadeBase SelecionarPorId(int id)
        {
            foreach (EntidadeBase entidade in registros) 
                if (entidade.Id == id) return entidade;
            return null;
        }

        public bool Existe(int id)
        {
            foreach(EntidadeBase entidade in registros) if (entidade != null)
                if (entidade.Id == id) return true;
            return false;
        }

        protected void RegistrarItem(EntidadeBase novoRegistro)
        {
            for (int i = 0; i < registros.Count; i++)
            {
                registros[i] = novoRegistro;
                break;
            }
        }
    }

}
