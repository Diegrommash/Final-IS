using BE;
using BE.Excepciones;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicioOrden
    {
        private readonly RepoOrden _repoOrden;
        public ServicioOrden()
        {
            _repoOrden = new RepoOrden();
        }

        public async Task<bool> AgregarOrdenAsync(Orden orden)
        {
            try
            {
                if (orden == null)
                {
                    throw new ArgumentNullException(nameof(orden), "La orden no puede ser nula");
                }
                if (string.IsNullOrWhiteSpace(orden.Declaracion))
                {
                    throw new ArgumentException("La declaración de la orden no puede estar vacía", nameof(orden.Declaracion));
                }

                var resultado = await _repoOrden.Agregar(orden);
                return resultado > 0;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al agregar la orden", ex);
            }
        }

        public async Task<bool> ModificarOrdenAsync(Orden orden)
        {
            try
            {
                if (orden == null)
                {
                    throw new ArgumentNullException(nameof(orden), "La orden no puede ser nula");
                }
                if (string.IsNullOrWhiteSpace(orden.Declaracion))
                {
                    throw new ArgumentException("La declaración de la orden no puede estar vacía", nameof(orden.Declaracion));
                }

                var resultado = await _repoOrden.Modificar(orden);
                return resultado;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al agregar la orden", ex);
            }
        }
        public async Task<bool> EliminarOrdenAsync(Orden orden)
        {
            try
            {
                if (orden == null)
                {
                    throw new ArgumentNullException(nameof(orden), "La orden no puede ser nula");
                }
                if (string.IsNullOrWhiteSpace(orden.Declaracion))
                {
                    throw new ArgumentException("La declaración de la orden no puede estar vacía", nameof(orden.Declaracion));
                }

                var resultado = await _repoOrden.Eliminar(orden);
                return resultado;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al agregar la orden", ex);
            }
        }
        public async Task<List<Orden>> BuscarTodosAsync()
        {
            try
            {
                var ordenes = await _repoOrden.Listar();
                if (ordenes == null || ordenes.Count == 0) return new List<Orden>();
               
                return ordenes;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar la orden", ex);
            }
        }

    }
}
