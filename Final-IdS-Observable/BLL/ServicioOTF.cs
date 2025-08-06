using BE.Excepciones;
using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicioOTF
    {
        private readonly RepoOrdenTrabajoFrase _repoOTF;
        public ServicioOTF()
        {
            _repoOTF = new RepoOrdenTrabajoFrase();
        }

        public async Task<bool> AgregarOTFAsync(OrdenTrabajoFrase otf)
        {
            try
            {
                if (otf.Orden == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La orden no puede ser nula");
                }
                if (otf.Trabajo == null)
                {
                    throw new ArgumentNullException(nameof(otf), "El trabajo no puede ser nulo");
                }
                if (otf == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La relacion no puede ser nula");
                }

                var resultado = await _repoOTF.Agregar(otf);
                return resultado > 0;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion(ex.Message, ex);
            }
        }

        public async Task<bool> EliminarOTFAsync(OrdenTrabajoFrase otf)
        {
            try
            {
                if (otf.Orden == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La orden no puede ser nula");
                }
                if (otf.Trabajo == null)
                {
                    throw new ArgumentNullException(nameof(otf), "El trabajo no puede ser nulo");
                }
                if (otf == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La relacion no puede ser nula");
                }

                var resultado = await _repoOTF.Eliminar(otf);
                return resultado;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al eliminar la relacion", ex);
            }
        }

        public async Task<bool> ModificarOTFAsync(OrdenTrabajoFrase otf)
        {
            try
            {
                if (otf.Orden == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La orden no puede ser nula");
                }
                if (otf.Trabajo == null)
                {
                    throw new ArgumentNullException(nameof(otf), "El trabajo no puede ser nulo");
                }
                if (otf == null)
                {
                    throw new ArgumentNullException(nameof(otf), "La relacion no puede ser nula");
                }

                var resultado = await _repoOTF.Modificar(otf);
                return resultado;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al modificar la relacion", ex);
            }
        }

        public async Task<List<OrdenTrabajoFrase>> BuscarTodosAsync()
        {
            try
            {
                var otfs = await _repoOTF.Listar();
                if (otfs == null || otfs.Count == 0) return new List<OrdenTrabajoFrase>();

                return otfs;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscr las relaciones", ex);
            }
        }

        public async Task<string?> ObtenerFrasePorOrdenYTrabajoAsync(string orden, string trabajo)
        {
            try
            {
                var todas = await _repoOTF.Listar();

                var match = todas.FirstOrDefault(o =>
                    o.Orden.Declaracion.Equals(orden, StringComparison.OrdinalIgnoreCase) &&
                    o.Trabajo.Nombre.Equals(trabajo, StringComparison.OrdinalIgnoreCase));

                return match?.Frase;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al obtener frase para la orden y trabajo", ex);
            }
        }
    }
}
