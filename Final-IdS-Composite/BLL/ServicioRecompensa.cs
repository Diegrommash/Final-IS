using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BE.Excepciones;
using DAL;

namespace BLL
{
    public class ServicioRecompensa
    {
        private readonly RepoRecompensa _repoRecompensa;
        public ServicioRecompensa()
        {
            _repoRecompensa = new RepoRecompensa(new Acceso());
        }
        public async Task<bool> AgregarRecompensa(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision), "La misión no puede ser nula.");
                if (mision.ObtenerRecompensas == null) throw new ArgumentNullException("El item de recompensa no puede ser nulo.");

                foreach (var item in mision.ObtenerRecompensas())
                {
                    var resultado = await _repoRecompensa.Agregar(mision.Id, item.Id);
                }

                return true;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al agregar la recompensa", ex);
            }

        }

        public async Task<bool> QuitarRecompensa(IMision mision, Item recompensa)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision), "La misión no puede ser nula.");
                if (recompensa == null) throw new ArgumentNullException(nameof(recompensa), "El item de recompensa no puede ser nulo.");

                var resultado = await _repoRecompensa.Eliminar(mision.Id, recompensa.Id);
                return resultado;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al Eliminar la recompensa", ex);
            }

        }

        public async Task<List<Item>> BuscarRecompensasMision(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision), "La misión no puede ser nula.");

                return await _repoRecompensa.BuscarRecompensasMision(mision.Id);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar las recompensas de la mision", ex);
            }

        }

        public async Task<IList<Item>> BuscarRecompensas()
        {
            try
            {
                return await _repoRecompensa.BuscarItems();
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar las recompensas de la mision", ex);
            }
        }

    }
}
