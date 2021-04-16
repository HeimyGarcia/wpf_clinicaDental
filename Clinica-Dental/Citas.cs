using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data;

namespace Clinica_Dental
{
    public enum EstadoCita
    {
        Activo = 1,
        Inactivo = 0
    }
    class Citas
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public int IdCita { get; set; }
        public int IdHistorialClinico { get; set; }
        public string Nota { get; set; }
        public DateTime FechaCita { get; set; }
        public TimeSpan Hora { get; set; }
        public EstadoCita EstadoCita { get; set; }

        //Constructores
        public Citas() { }
        public Citas(int idCita, int idHistorialClinico, string nota,  DateTime fechaCita, TimeSpan hora, EstadoCita status)
        {
            IdCita = idCita;
            IdHistorialClinico = idHistorialClinico;
            Nota = nota;
            FechaCita = fechaCita;
            Hora = hora;
            EstadoCita = status;
        }

        //
        private int ObtenerEstado(EstadoCita estadoCita)
        {
            switch (estadoCita)
            {
                case EstadoCita.Activo:
                    return 1;
                case EstadoCita.Inactivo:
                    return 0;
                default:
                    return 1;
            }
        }

        private int CambiarEstado(EstadoCita estadoCita)
        {
            switch (estadoCita)
            {
                case EstadoCita.Activo:
                    return 0;
                case EstadoCita.Inactivo:
                    return 0;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Inserta una cita en la base de datos.
        /// </summary>
        /// <param name="citas">Información del cita de ingresar</param>
        public void AgregarCita(Citas citas)
        {
            try
            {
                // Query de inserción
                string query = @"insert into[Pacientes].[Cita](idHistorialClinico, fechaCita, hora, nota, estado)
                        values(@idHistorialClinico, @fechaCita, @hora, @nota, @estado)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros;
                sqlCommand.Parameters.AddWithValue("@idCita", citas.IdCita);
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", citas.IdHistorialClinico);
                sqlCommand.Parameters.AddWithValue("@nota", citas.Nota);
                sqlCommand.Parameters.AddWithValue("@fechaCita", citas.FechaCita.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@hora", citas.Hora.ToString());
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(citas.EstadoCita));

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
        /// Mostrar la lista de información de los citas.
        /// </summary>
        /// <returns>Lista de citas.</returns>
        public List<Citas> MostrarCitas(int idHistorial)
        {
            //Inicializar una lista vacía de las citas
            List<Citas> citas = new List<Citas>();

            try
            {
                // Query de selección
                string query = @"select * from [Pacientes].[Cita] where idHistorialClinico = @idHistorialClinico";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@idHistorialClinico", idHistorial);

                // Obtener los datos de la cita
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        citas.Add(new Citas
                        {
                            IdCita = Convert.ToInt32(rdr["idCita"]),
                            IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]),
                            Nota = rdr["nota"].ToString(),
                            FechaCita = Convert.ToDateTime(rdr["fechaCita"]),
                            Hora = TimeSpan.Parse((string)rdr["hora"].ToString()),
                            EstadoCita = (EstadoCita)Convert.ToInt32((rdr["estado"]))
                        });
                    }
                }
                return citas;
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
        /// Modifica una cita de la base de datos.
        /// </summary>
        /// <param name="cita">Información del pacientes a modificar</param>
        public void ModificarCita(Citas cita)
        {
            try
            {
                // Query de actualización
                string query = @"update [Pacientes].[cita]
                                set  fechaCita = @fechaCita, nota = @nota, hora = @hora, estado = @estado
                                where idCita = @idCita";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idCita", cita.IdCita);
                sqlCommand.Parameters.AddWithValue("@nota", cita.Nota);
                sqlCommand.Parameters.AddWithValue("@fechaCita", cita.FechaCita.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@hora", cita.Hora.ToString());
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(cita.EstadoCita));

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
        /// Modifica el estado de los datos de la cita.
        /// </summary>
        /// <param name="cita">Información de la cita a eliminar</param>
        public void EliminarCita(Citas cita)
        {
            try
            {
                // Query de eliminación
                string query = @"update [Pacientes].[Cita]
                                 set estado = @estado
                                 WHERE idCita = @idCita";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idCita", cita.IdCita);
                sqlCommand.Parameters.AddWithValue("@estado", CambiarEstado(cita.EstadoCita));

                // Ejecutar el comando de eliminación
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
        /// Buscar una cita por su id
        /// </summary>
        /// <param name="cita">Id del cita a buscar</param>
        /// <returns>Cita si es encontrado</returns>
        public Citas BuscarCita(int cita)
        {
            Citas lacita = new Citas();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM [Pacientes].[Cita]
                                 WHERE idCita = @idCita";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@idCita", cita);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lacita.IdCita = Convert.ToInt32(rdr["idCita"]);
                        lacita.IdHistorialClinico = Convert.ToInt32(rdr["idHistorialClinico"]);
                        lacita.Nota = rdr["nota"].ToString();
                        lacita.FechaCita = Convert.ToDateTime(rdr["fechaCita"]);
                        lacita.Hora = TimeSpan.Parse((string)rdr["hora"].ToString());
                        lacita.EstadoCita = (EstadoCita)Convert.ToInt32((rdr["estado"]));
                    }
                }

                return lacita;
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

        public int BuscarHora(Citas citas)
        {
            int count = 0;
            try
            {
                // Query de búsqueda
                string query = @"SELECT count(*) as total FROM [Pacientes].[Cita]
                                 WHERE fechaCita = @fechaCita and hora = @hora";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@fechaCita", citas.FechaCita.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@hora", citas.Hora.ToString());

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        count = Convert.ToInt32(rdr["total"]);
                    }
                }

                return count;
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
