using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;

namespace Clinica_Dental
{
    public enum estadoConsultaTratamiento
    {
        Activo = 1,
        Inactivo = 0
    }
    class ConsultaTratamiento
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Propiedades
        public int IdTratamiento { get; set; }

        public int IdHistorialConsulta { get; set; }

        public estadoConsultaTratamiento Estado { get; set; }

        // Constructores
        public ConsultaTratamiento() { }

        public ConsultaTratamiento(int idTratamiento, int idHistorialConsulta, estadoConsultaTratamiento estado)
        {
            IdTratamiento = idTratamiento;
            IdHistorialConsulta = idHistorialConsulta;
            Estado = estado;
        }
        private int ObtenerEstado(estadoConsultaTratamiento estado)
        {
            switch (estado)
            {
                case estadoConsultaTratamiento.Activo:
                    return 1;
                case estadoConsultaTratamiento.Inactivo:
                    return 0;
                default:
                    return 1;
            }
        }
        private int CambiarEstado(estadoConsultaTratamiento estado)
        {
            switch (estado)
            {
                case estadoConsultaTratamiento.Activo:
                    return 0;
                case estadoConsultaTratamiento.Inactivo:
                    return 0;
                default:
                    return 0;
            }
        }

        //Metodos
        /// <summary>
        /// Inserta una Tratamiento.
        /// </summary>
        /// <param name="tratamiento">La información del tratamiento</param>
        public void CrearTratamiento(ConsultaTratamiento tratamiento)
        {
            try
            {
                // Query de inserción
                string query = @"INSERT INTO Pacientes.ConsultaTratamiento (idTratamiento, idHistorialConsulta, estado)
                                 VALUES (@idTratamiento, @idHistorialConsulta,@estado)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idTratamiento", tratamiento.IdTratamiento);
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", tratamiento.IdHistorialConsulta);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(tratamiento.Estado));

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
        /// Muestra todos los Tratamiento
        /// </summary>
        /// <returns>Un listado de los Tratamiento</returns>
        public List<ConsultaTratamiento> MostrarTratamiento(int idHistorialConsulta)
        {
            // Inicializar una lista vacía de tratamientos
            List<ConsultaTratamiento> tratamientos = new List<ConsultaTratamiento>();

            try
            {
                // Query de selección
                string query = @"SELECT * FROM Pacientes.ConsultaTratamiento WHERE idHistorialConsulta=@idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", idHistorialConsulta);

                // Obtener los datos de las habitaciones
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        tratamientos.Add(new ConsultaTratamiento
                        {
                            IdTratamiento = Convert.ToInt32(rdr["idTratamiento"]),
                            IdHistorialConsulta = Convert.ToInt32(rdr["idHistorialConsulta"]),
                            Estado = (estadoConsultaTratamiento)Convert.ToInt32((rdr["estado"])) //probar
                        });
                    }
                }
                return tratamientos;

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
        /// Obtiene un detalle por su Tratamiento
        /// </summary>
        /// <param name="idTratamiento">El idTratamiento del DetalleTratamiento</param>
        /// <param name="idHistorialConsulta">El idHistorialConsulta del Tratamiento</param>
        /// <returns>Los datos del DetalleTratamiento</returns>
        public ConsultaTratamiento BuscarTratamiento(int idTratamiento, int idHistorialConsulta)
        {
            ConsultaTratamiento elTratamiento = new ConsultaTratamiento();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM Pacientes.ConsultaTratamiento
                                 WHERE idTratamiento = @idTratamiento and idHistorialConsulta=@idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idTratamiento", idTratamiento);
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", idHistorialConsulta);
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        elTratamiento.IdTratamiento = Convert.ToInt32(rdr["idTratamiento"]);
                        elTratamiento.IdHistorialConsulta = Convert.ToInt32(rdr["idHistorialConsulta"]);
                        elTratamiento.Estado = (estadoConsultaTratamiento)Convert.ToInt32((rdr["estado"])); //probar
                    }
                }

                return elTratamiento;
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
        /// Modifica los datos de Tratamiento
        /// </summary>
        /// <param name="tratamiento">Informacion de Tratamiento</param>
        public void ModificarTratamiento(ConsultaTratamiento tratamiento)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.ConsultaTratamiento
                                 SET idTratamiento = @idTratamiento, idHistorialConsulta = @idHistorialConsulta, estado = @estado
                                 WHERE idTratamiento = @idTratamiento and idHistorialConsulta=@idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idTratamiento", tratamiento.IdTratamiento);
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", tratamiento.IdHistorialConsulta);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(tratamiento.Estado));

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
        /// Elimina un Tratamiento
        /// </summary>
        /// <param name="tratamiento">La informacion de tratamiento</param>
        public void EliminarTratamiento(ConsultaTratamiento tratamiento)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.ConsultaTratamiento
                                 SET estado = @estado
                                 WHERE idTratamiento = @idTratamiento and idHistorialConsulta=@idHistorialConsulta";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@estado", CambiarEstado(tratamiento.Estado));
                sqlCommand.Parameters.AddWithValue("@idTratamiento", tratamiento.IdTratamiento);
                sqlCommand.Parameters.AddWithValue("@idHistorialConsulta", tratamiento.IdHistorialConsulta);

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
