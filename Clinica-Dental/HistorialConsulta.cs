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
    public enum estadoHistorial
    {
        Activo = 1,
        Inactivo = 0
    }
    class HistorialConsulta
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Propiedades
        public int IdHistorialConsulta { get; set; }

        public int IdHistorialClinico { get; set; }

        public DateTime FechaConsulta { get; set; }

        public string MotivoConsulta { get; set; }
        public string Observaciones { get; set; }
        public string IdentidadEmpleado { get; set; }
        public estadoHistorial Estado { get; set; }

        // Constructores
        public HistorialConsulta() { }

        public HistorialConsulta(int idHistorialClinico, string motivoConsulta, string observaciones, string identidadEmpleado, estadoHistorial estado)
        {
            IdHistorialClinico = idHistorialClinico;
            FechaConsulta = DateTime.Now;
            MotivoConsulta = motivoConsulta;
            Observaciones = observaciones;
            IdentidadEmpleado = identidadEmpleado;
            Estado = estado;
        }
        private int ObtenerEstado(estadoHistorial estadoHistorial)
        {
            switch (estadoHistorial)
            {
                case estadoHistorial.Activo:
                    return 1;
                case estadoHistorial.Inactivo:
                    return 0;
                default:
                    return 1;
            }
        }
        private int CambiarEstado(estadoHistorial estadoHistorial)
        {
            switch (estadoHistorial)
            {
                case estadoHistorial.Activo:
                    return 0;
                case estadoHistorial.Inactivo:
                    return 0;
                default:
                    return 0;
            }
        }

        //Metodos
        /// <summary>
        /// Inserta una Consulta.
        /// </summary>
        /// <param name="historialConsulta">La información del detalleTratamiento</param>
        public void CrearHistorialConsulta(HistorialConsulta historialConsulta)
        {
            try
            {
                // Query de inserción
                string query = @"INSERT INTO Pacientes.HistorialConsulta (idHistorialClinico, fechaConsulta, motivoConsulta, observaciones, identidadEmpleado,estado)
                                 VALUES (@idHistorialClinico, @fechaConsulta, @motivoConsulta,@observaciones,@identidadEmpleado,@estado)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", historialConsulta.IdHistorialClinico);
                sqlCommand.Parameters.AddWithValue("@fechaConsulta", historialConsulta.FechaConsulta);
                sqlCommand.Parameters.AddWithValue("@motivoConsulta", historialConsulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@observaciones", historialConsulta.Observaciones);
                sqlCommand.Parameters.AddWithValue("@identidadEmpleado", historialConsulta.IdentidadEmpleado);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(historialConsulta.Estado));

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
        public List<HistorialConsulta> MostrarHistorialConsulta()
        {
            // Inicializar una lista vacía de habitaciones
            List<HistorialConsulta> consultas = new List<HistorialConsulta>();

            try
            {
                // Query de selección
                string query = @"SELECT * FROM Pacientes.HistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Obtener los datos de las de la consulta
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        consultas.Add(new HistorialConsulta
                        {
                            IdHistorialConsulta = Convert.ToInt32(rdr["idHistorialConsulta"]),
                            IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]),
                            FechaConsulta = Convert.ToDateTime(rdr["fechaConsulta"]).Date,
                            MotivoConsulta = rdr["motivoConsulta"].ToString(),
                            Observaciones = rdr["observaciones"].ToString(),
                            IdentidadEmpleado = rdr["identidadEmpleado"].ToString(),
                            Estado = (estadoHistorial)Convert.ToInt32((rdr["estado"])) //probar
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
        /// <param name="idHistorialConsulta">El idHistorialConsulta del HistorialConsulta</param>
        /// <returns>Los datos del HistorialConsulta</returns>
        public HistorialConsulta BuscarHistorialConsulta(int idHistorialConsulta)
        {
            HistorialConsulta elhistorialConsulta = new HistorialConsulta();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM Pacientes.HistorialConsulta
                                 WHERE idHistorialConsulta = @idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", idHistorialConsulta);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        elhistorialConsulta.IdHistorialConsulta = Convert.ToInt32(rdr["idHistorialConsulta"]);
                        elhistorialConsulta.IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]);
                        elhistorialConsulta.IdentidadEmpleado = rdr["identidadEmpleado"].ToString();
                        elhistorialConsulta.MotivoConsulta = rdr["motivoConsulta"].ToString();
                        elhistorialConsulta.Observaciones = rdr["observaciones"].ToString();
                        elhistorialConsulta.Estado = (estadoHistorial)Convert.ToInt32((rdr["estado"])); //probar
                    }
                }

                return elhistorialConsulta;
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
        /// Modifica los datos de la consulta
        /// </summary>
        /// <param name="historialConsulta">Informacion de la consulta</param>
        /// <summary>
        /// Modifica los datos de la consulta
        /// </summary>
        /// <param name="historialConsulta">Informacion de la consulta</param>
        public void ModificarHistorialConsulta(HistorialConsulta historialConsulta)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.HistorialConsulta
                                 SET motivoConsulta = @motivoConsulta, observaciones = @observaciones, identidadEmpleado=@identidadEmpleado, estado= @estado
                                 WHERE idHistorialConsulta = @idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", historialConsulta.IdHistorialConsulta);
                sqlCommand.Parameters.AddWithValue("@motivoConsulta", historialConsulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@observaciones", historialConsulta.Observaciones);
                sqlCommand.Parameters.AddWithValue("@identidadEmpleado", historialConsulta.IdentidadEmpleado);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(historialConsulta.Estado));

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
        public void EliminarHistorialConsulta(HistorialConsulta historialConsulta)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.HistorialConsulta
                                 SET estado = @estado
                                 WHERE idHistorialConsulta = @idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@estado", CambiarEstado(historialConsulta.Estado));
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", historialConsulta.IdHistorialConsulta);

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
