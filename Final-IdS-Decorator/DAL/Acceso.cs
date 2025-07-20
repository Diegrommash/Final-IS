using BE.Excepciones;
using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class Acceso
    {
        private readonly string? _cadenaConexion;
        private SqlConnection? _conexion;
        private SqlTransaction? _transaccion;

        public Acceso()
        {
            _cadenaConexion = Configuracion.ObtenerCadenaConexion("EnCasa");

            if (string.IsNullOrWhiteSpace(_cadenaConexion))
                throw new InvalidOperationException("La cadena de conexión está vacía o no fue configurada.");
        }

        private async Task<SqlConnection> ObtenerConexionAsync()
        {
            if (_transaccion != null)
            {
                if (_conexion == null || _conexion.State != ConnectionState.Open)
                {
                    _conexion = new SqlConnection(_cadenaConexion);
                    await _conexion.OpenAsync();
                }

                return _conexion;
            }
            else
            {
                SqlConnection conexion = new SqlConnection(_cadenaConexion);
                await conexion.OpenAsync();
                return conexion;
            }
        }

        public async Task ComenzarTransaccionAsync()
        {
            _conexion = new SqlConnection(_cadenaConexion);
            await _conexion.OpenAsync();
            _transaccion = (SqlTransaction?)await _conexion.BeginTransactionAsync();
        }

        public async Task ConfirmarTransaccionAsync()
        {
            _transaccion?.Commit();
            _transaccion = null;
            await CerrarConexionAsync();
        }

        public async Task CancelarTransaccionAsync()
        {
            _transaccion?.Rollback();
            _transaccion = null;
            await CerrarConexionAsync();
        }

        private async Task CerrarConexionAsync()
        {
            if (_conexion != null && _conexion.State != ConnectionState.Closed)
            {
                await _conexion.CloseAsync();
                await _conexion.DisposeAsync();
                _conexion = null;
            }
        }

        public IDbCommand CrearComando(string sql, List<IDbDataParameter>? parametros = null, CommandType tipoCmd = CommandType.StoredProcedure)
        {
            if (_conexion == null)
                throw new InvalidOperationException("No hay conexión abierta para crear el comando.");

            SqlCommand cmd = _conexion.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = tipoCmd;

            if (parametros != null)
            {
                foreach (var param in parametros)
                {
                    var sqlParam = (SqlParameter)param;
                    var nuevoParam = (SqlParameter)((ICloneable)sqlParam).Clone();
                    cmd.Parameters.Add(nuevoParam);
                }
            }

            if (_transaccion != null)
                cmd.Transaction = _transaccion;

            return cmd;
        }

        public IDbDataParameter CrearParametro(string nombre, object valor, DbType tipo, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                ParameterName = nombre,
                Value = valor ?? DBNull.Value,
                DbType = tipo,
                Direction = direction
            };
        }


        public async Task<object?> DevolverEscalarAsync(string nombreSp, List<IDbDataParameter>? parametros = null, CommandType tipoCmd = CommandType.StoredProcedure)
        {
            SqlConnection conn = await ObtenerConexionAsync();
            try
            {
                using SqlCommand cmd = new(nombreSp, conn)
                {
                    CommandType = tipoCmd,
                    Transaction = _transaccion
                };

                if (parametros != null)
                    cmd.Parameters.AddRange(parametros.Cast<SqlParameter>().ToArray());

                return await cmd.ExecuteScalarAsync();
            }
            catch(SqlException ex)
            {
                throw new AccesoADatosExcepcion($"Error al devolver escalar: {ex.Message}");
            }
            finally
            {
                if (_transaccion == null)
                    await conn.DisposeAsync();
            }
        }

        public async Task<int> EscribirAsync(string nombreSp, List<IDbDataParameter>? parametros = null, CommandType tipoCmd = CommandType.StoredProcedure)
        {
            SqlConnection conn = await ObtenerConexionAsync();
            try
            {
                using SqlCommand cmd = new(nombreSp, conn)
                {
                    CommandType = tipoCmd,
                    Transaction = _transaccion
                };

                if (parametros != null)
                    cmd.Parameters.AddRange(parametros.Cast<SqlParameter>().ToArray());

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new AccesoADatosExcepcion($"Error al escribir: {ex.Message}");
            }
            finally
            {
                if (_transaccion == null)
                    await conn.DisposeAsync();
            }
        }

        public async Task<DataTable?> LeerAsync(string nombreSp, List<IDbDataParameter>? parametros = null, CommandType tipoCmd = CommandType.StoredProcedure)
        {
            SqlConnection conn = await ObtenerConexionAsync();
            try
            {
                using SqlCommand cmd = new(nombreSp, conn)
                {
                    CommandType = tipoCmd,
                    Transaction = _transaccion
                };

                if (parametros != null)
                    cmd.Parameters.AddRange(parametros.Cast<SqlParameter>().ToArray());

                using var reader = await cmd.ExecuteReaderAsync();
                var tabla = new DataTable();
                tabla.Load(reader);
                return tabla;
            }
            catch (SqlException ex)
            {
                throw new AccesoADatosExcepcion($"Error al leer: {ex.Message}");
            }
            finally
            {
                if (_transaccion == null)
                    await conn.DisposeAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CerrarConexionAsync();
        }
    }
}

