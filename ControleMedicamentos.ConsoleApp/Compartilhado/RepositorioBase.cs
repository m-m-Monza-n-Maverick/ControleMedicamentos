namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal abstract class RepositorioBase
    {
        private EntidadeBase[] registros = new EntidadeBase[100];
        private int contadorId = 0;

        public void Cadastrar(EntidadeBase novoRegistro)
        {
            contadorId++;
            novoRegistro.Id = contadorId;
            RegistrarItem(novoRegistro);
        }

        public void Editar(int id, EntidadeBase novaEntidade)
        {
            novaEntidade.Id = id;
            registros[id - 1] = novaEntidade;
        }

        public void Excluir(int id) => registros[id - 1] = null;

        public bool ExistemItensCadastrados() => contadorId != 0;

        public EntidadeBase[] SelecionarTodos()
        {
            return registros;
        }

        public EntidadeBase SelecionarPorId(int id)
        {
            for (int i = 0; i < registros.Length; i++)
            {
                EntidadeBase e = registros[i];

                if (e == null) continue;

                else if (e.Id == id) return e;
            }
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
            for (int i = 0; i < registros.Length; i++)
            {
                if (registros[i] != null)
                    continue;

                else
                {
                    registros[i] = novoRegistro;
                    break;
                }
            }
        }
    }

}
