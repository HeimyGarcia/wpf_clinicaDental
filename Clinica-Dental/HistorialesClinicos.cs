using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.ComponentModel;


namespace Clinica_Dental
{
    public enum estadoClinico
    {
        Activo = 1,
        Inactivo = 0
    }

    class HistorialesClinicos
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Propiedades

        public int IdHistorialClinico { get; set; }

        public string identidadPaciente { get; set; }

        public DateTime FechaCreacion { get; set; }
        [DisplayName("Fecha")]

        public string Observaciones { get; set; }
        public string Afecciones { get; set; }
        public estadoClinico Estado { get; set; }

        // Constructores
        public HistorialesClinicos() { }

        public HistorialesClinicos(int idHistorialClinico, string IdentidadPaciente, string observaciones, string afecciones, estadoClinico estado)
        {
            IdHistorialClinico = idHistorialClinico;
            identidadPaciente = IdentidadPaciente;
            FechaCreacion = DateTime.Now;
            Observaciones = observaciones;
            Afecciones = afecciones;
            Estado = estado;
        }
        private int ObtenerEstado(estadoClinico estado)
        {
            switch (estado)
            {
                case estadoClinico.Activo:
                    return 1;
                case estadoClinico.Inactivo:
                    return 0;
                default:
                    return 1;
            }
        }
        private int CambiarEstado(estadoClinico estado)
        {
            switch (estado)
            {
                case estadoClinico.Activo:
                    return 0;
                case estadoClinico.Inactivo:
                    return 0;
                default:
                    return 0;
            }
        }

        //Metodos
        /// <summary>
        /// Inserta un Historial.
        /// </summary>
        /// <param name="historialClinico">La información del historialClinico</param>
        public void CrearHistorialClinico(HistorialesClinicos historialClinico)
        {
            try
            {
                // Query de inserción
                string query = @"INSERT INTO Pacientes.HistorialClinico (idHistorialClinico, identidadPaciente, fechaCreacion, observaciones, afecciones,estado)
                                 VALUES (@idHistorialClinico, @identidadPaciente, @fechaCreacion,@observaciones,@afecciones,@estado)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", historialClinico.IdHistorialClinico);
                sqlCommand.Parameters.AddWithValue("@identidadPaciente", historialClinico.identidadPaciente);
                sqlCommand.Parameters.AddWithValue("@fechaCreacion", historialClinico.FechaCreacion);
                sqlCommand.Parameters.AddWithValue("@observaciones", historialClinico.Observaciones);
                sqlCommand.Parameters.AddWithValue("@afecciones", historialClinico.Afecciones);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(historialClinico.Estado));

                // Ejecutar el comando de inserción
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Muestra todas las consultas
        /// </summary>
        /// <returns>Un listado de las consultas</returns>
        public List<HistorialesClinicos> MostrarHistorialClinico()
        {
            // Inicializar una lista vacía de habitaciones
            List<HistorialesClinicos> consultas = new List<HistorialesClinicos>();

            try
            {
                // Query de selección
                string query = @"SELECT * FROM Pacientes.HistorialClinico";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Obtener los datos de las de la consulta
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        consultas.Add(new HistorialesClinicos
                        {
                            IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]),
                            identidadPaciente = rdr["identidadPaciente"].ToString(),
                            FechaCreacion = Convert.ToDateTime(rdr["fechaCreacion"]).Date,
                            Observaciones = rdr["observaciones"].ToString(),
                            Afecciones = rdr["afecciones"].ToString(),
                            Estado = (estadoClinico)Convert.ToInt32((rdr["estado"])) //probar
                        });
                    }
                }
                return consultas;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Obtiene un detalle por su idHistorialConsulta
        /// </summary>
        /// <param name="idHistorialClinico">El idHistorialConsulta del HistorialConsulta</param>
        /// <returns>Los datos del HistorialConsulta</returns>
        public HistorialesClinicos BuscarHistorialClinico(int idHistorialClinico)
        {
            HistorialesClinicos elhistorialClinico = new HistorialesClinicos();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM Pacientes.HistorialClinico
                                 WHERE idHistorialClinico = @idHistorialClinico";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", idHistorialClinico);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        elhistorialClinico.IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]);
                        elhistorialClinico.identidadPaciente = rdr["identidadPaciente"].ToString();
                        elhistorialClinico.Afecciones = rdr["afecciones"].ToString();
                        elhistorialClinico.Observaciones = rdr["observaciones"].ToString();
                        elhistorialClinico.Estado = (estadoClinico)Convert.ToInt32((rdr["estado"])); //probar
                    }
                }

                return elhistorialClinico;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Modifica los datos del historial
        /// </summary>
        /// <param name="historialClinico">Informacion del historial</param>
        public void ModificarHistorialClinico(HistorialesClinicos historialClinico)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.HistorialClinico
                                 SET idHistorialClinico = @idHistorialClinico, fechaCreacion = @fechaCreacion, afecciones = @afecciones, observaciones = @observaciones, identidadPaciente=@identidadPaciente ,estado= @estado
                                 WHERE idHistorialClinico = @idHistorialClinico";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", historialClinico.IdHistorialClinico);
                sqlCommand.Parameters.AddWithValue("@fechaCreacion", historialClinico.FechaCreacion);
                sqlCommand.Parameters.AddWithValue("@afecciones", historialClinico.Afecciones);
                sqlCommand.Parameters.AddWithValue("@observaciones", historialClinico.Observaciones);
                sqlCommand.Parameters.AddWithValue("@identidadPaciente", historialClinico.identidadPaciente);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(historialClinico.Estado));

                // Ejecutar el comando de actualización
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Elimina una consulta
        /// </summary>
        /// <param name="historialConsulta">La informacion de la consulta</param>
        public void EliminarHistorialConsulta(HistorialesClinicos historialClinico)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.HistorialClinico
                                 SET estado = @estado
                                 WHERE idHistorialClinico = @idHistorialClinico";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@estado", CambiarEstado(historialClinico.Estado));
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", historialClinico.IdHistorialClinico);

                // Ejecutar el comando de actualización
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }
    }
}