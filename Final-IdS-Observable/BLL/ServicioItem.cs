using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BE.Excepciones;
using DAL;

namespace BLL
{
    public class ServicioItem
    {
        private readonly RepoItem _repoItem;

        public ServicioItem()
        {
            _repoItem = new RepoItem(new Acceso());
        }

        public async Task<IList<Item>> BuscarTodos()
        {
            try
            {
                var items = await _repoItem.BuscarTodos();
                return items;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar los items", ex);
            }
           
        }
    }
}
