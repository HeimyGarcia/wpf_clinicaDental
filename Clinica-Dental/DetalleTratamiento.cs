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
    public enum estado
    {
        Activo = 1,
        Inactivo = 0
    }
    class DetalleTratamiento
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);
        private int valor = 0;

        // Propiedades
        public int IdTratamiento { get; set; }

        public string NombreTratamiento { get; set; }

        public string DuracionTratamiento { get; set; }

        public string Indicaciones { get; set; }
        public decimal Precio { get; set; }

        public estado Estado { get; set; }

        // Constructores
        public DetalleTratamiento() { }

        public DetalleTratamiento(string nombreTratamiento, string duracionTratamiento, string indicaciones, decimal precio, estado estado)
        {
            NombreTratamiento = nombreTratamiento;
            DuracionTratamiento = duracionTratamiento;
            Indicaciones = indicaciones;
            Precio = precio;
            Estado = estado;
        }

        private int ObtenerEstado(estado estado)
        {
            switch (estado)
            {
                case estado.Activo:
                    return 1;
                case estado.Inactivo:
                    return 0;
                default:
                    return 1;
            }
        }
        private int CambiarEstado(estado estado)
        {
            switch (estado)
            {
                case estado.Activo:
                    return 0;
                case estado.Inactivo:
                    return 0;
                default:
                    return 0;
            }
        }

        //Metodos
        /// <summary>
        /// Inserta una DetalleTratamiento.
        /// </summary>
        /// <param name="detalleTratamiento">La información del detalleTratamiento</param>
        public void CrearDetalleTratamiento(DetalleTratamiento detalleTratamiento)
        {
            try
            {
                // Query de inserción
                string query = @"INSERT INTO Pacientes.DetalleTratamiento (nombreTratamiento, duracionTratamiento, indicaciones, precio, estado)
                                 VALUES (@nombreTratamiento, @duracionTratamiento, @indicaciones,@precio,@estado)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@nombreTratamiento", detalleTratamiento.NombreTratamiento);
                sqlCommand.Parameters.AddWithValue("@duracionTratamiento", detalleTratamiento.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicaciones", detalleTratamiento.Indicaciones);
                sqlCommand.Parameters.AddWithValue("@precio", detalleTratamiento.Precio);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(detalleTratamiento.Estado));

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
        /// Muestra todas los detalleTratamiento
        /// </summary>
        /// <returns>Un listado de los detalleTratamiento</returns>
        public List<DetalleTratamiento> MostrarDetalleTratamiento()
        {
            // Inicializar una lista vacía de detalleTratamiento
            List<DetalleTratamiento> detalleTratamientos = new List<DetalleTratamiento>();

            try
            {
                // Query de selección
                string query = @"SELECT * FROM Pacientes.DetalleTratamiento";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Obtener los datos de los detalleTratamiento
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        detalleTratamientos.Add(new DetalleTratamiento
                        {
                            IdTratamiento = Convert.ToInt32(rdr["idTratamiento"]),
                            NombreTratamiento = rdr["nombreTratamiento"].ToString(),
                            DuracionTratamiento = rdr["duracionTratamiento"].ToString(),
                            Precio = Convert.ToDecimal(rdr["precio"]),
                            Indicaciones = rdr["indicaciones"].ToString(),
                            Estado = (estado)Convert.ToInt32((rdr["estado"])) //probar
                        });
                    }
                }
                return detalleTratamientos;

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
        /// Obtiene un detalle por su idTratamiento
        /// </summary>
        /// <param name="idTratamiento">El idTratamiento del DetalleTratamiento</param>
        /// <returns>Los datos del DetalleTratamiento</returns>
        public DetalleTratamiento BuscarDetalleTratamiento(int idTratamiento)
        {
            DetalleTratamiento elDetalleTratamiento = new DetalleTratamiento();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM Pacientes.DetalleTratamiento
                                 WHERE idTratamiento = @idTratamiento";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idTratamiento", idTratamiento);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        elDetalleTratamiento.IdTratamiento = Convert.ToInt32(rdr["idTratamiento"]);
                        elDetalleTratamiento.NombreTratamiento = rdr["nombreTratamiento"].ToString();
                        elDetalleTratamiento.DuracionTratamiento = rdr["duracionTratamiento"].ToString();
                        elDetalleTratamiento.Indicaciones = rdr["indicaciones"].ToString();
                        elDetalleTratamiento.Precio = Convert.ToDecimal(rdr["precio"]);
                        elDetalleTratamiento.Estado = (estado)Convert.ToInt32((rdr["estado"])); //probar
                    }
                }

                return elDetalleTratamiento;
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
        /// Modifica los datos de DetalleTratamiento
        /// </summary>
        /// <param name="detalleTratamiento">Informacion de DetalleTratamiento</param>
        public void ModificarDetalleTratamiento(DetalleTratamiento detalleTratamiento)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.DetalleTratamiento
                                 SET nombreTratamiento = @nombreTratamiento, duracionTratamiento = @duracionTratamiento, indicaciones = @indicaciones, precio = @precio, estado = @estado
                                 WHERE idTratamiento = @idTratamiento";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idTratamiento", detalleTratamiento.IdTratamiento);
                sqlCommand.Parameters.AddWithValue("@nombreTratamiento", detalleTratamiento.NombreTratamiento);
                sqlCommand.Parameters.AddWithValue("@duracionTratamiento", detalleTratamiento.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicaciones", detalleTratamiento.Indicaciones);
                sqlCommand.Parameters.AddWithValue("@precio", detalleTratamiento.Precio);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(detalleTratamiento.Estado));

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
        /// Elimina una DetalleTratamiento
        /// </summary>
        /// <param name="detalleTratamiento">La informacion de DetalleTratamiento</param>
        public void EliminarDetalleTratamiento(DetalleTratamiento detalleTratamiento)
        {
            try
            {
                // Query de actualización
                string query = @"UPDATE Pacientes.DetalleTratamiento
                                 SET estado = @estado
                                 WHERE idTratamiento = @idTratamiento";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@estado", CambiarEstado(detalleTratamiento.Estado));
                sqlCommand.Parameters.AddWithValue("@idTratamiento", detalleTratamiento.IdTratamiento);

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
