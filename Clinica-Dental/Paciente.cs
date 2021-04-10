using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace Clinica_Dental
{
    public enum Sexo
    {
        Masculino = 'M',
        Femenino = 'F'
    }

    class Paciente
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public string Identidad { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Sexo SexoPaciente { get; set; }

        //Constructores
        public Paciente() { }
        public Paciente(string identidad, string nombres, string apellidos, string direccion, string correo, string celular, DateTime fecha, Sexo sexo)
        {
            Identidad = identidad;
            Nombres = nombres;
            Apellidos = apellidos;
            Direccion = direccion;
            Correo = correo;
            Celular = celular;
            FechaNacimiento = fecha;
            SexoPaciente = sexo;
        }

        private string ObtenerSexo(Sexo sexo)
        {
            switch (sexo)
            {
                case Sexo.Masculino:
                    return "Masculino";
                case Sexo.Femenino:
                    return "Femenino";
                default:
                    return "Masculino";
            }
        }


        /// <summary>
        /// Inserta un paciente en la base de datos.
        /// </summary>
        /// <param name="paciente">Información del paciente de ingresar</param>
        public void AgregarPaciente(Paciente paciente)
        {
            try
            {
                // Query de inserción
                string query = @"insert into [Pacientes].[Paciente]
                                values ('@identidad','@nombres','@apellidos','@direccion',
                                '@correo','@celular','@fechaNacimiento','@sexo')";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros;
                sqlCommand.Parameters.AddWithValue("@identidad", paciente.Identidad);
                sqlCommand.Parameters.AddWithValue("@nombres", paciente.Nombres);
                sqlCommand.Parameters.AddWithValue("@apellidos", paciente.Apellidos);
                sqlCommand.Parameters.AddWithValue("@direccion", paciente.Direccion);
                sqlCommand.Parameters.AddWithValue("@correo", paciente.Correo);
                sqlCommand.Parameters.AddWithValue("@celular", paciente.Celular);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", paciente.FechaNacimiento.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@sexo", ObtenerSexo(paciente.SexoPaciente));

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
        /// Mostrar la lista de información de los pacientes.
        /// </summary>
        /// <returns>Lista de pacientes.</returns>
        public List<Paciente> MostrarPaciente()
        {
            //Inicializar una lista vacía de los Pacientes
            List<Paciente> pacientes = new List<Paciente>();

            try
            {
                // Query de selección
                string query = @"select * from [Pacientes].[Paciente]";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Obtener los datos del paciente
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read()) {
                        pacientes.Add(new Paciente
                        {
                            Identidad = rdr["identidad"].ToString(),
                            Nombres = rdr["nombres"].ToString(),
                            Apellidos = rdr["apellidos"].ToString(),
                            Correo = rdr["correoElectronico"].ToString(),
                            Celular = rdr["celular"].ToString(),
                            Direccion = rdr["direccion"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(rdr["fechaNacimiento"]),
                            SexoPaciente = (Sexo)Convert.ToChar(rdr["sexo"].ToString().Substring(0, 1))
                        });
                    }
                }
                return pacientes;
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
        /// Modifica un paciente de la base de datos.
        /// </summary>
        /// <param name="paciente">Información del pacientes a modificar</param>
        public void ModificarPaciente(Paciente paciente)
        {
            try
            {
                // Query de actualización
                string query = @"update [Pacientes].[Paciente]
                                set  nombres = @nombres,apellidos = @apellidos,direccion = @direccion,
                                correoElectronico = @correoElectronico,celular = @celular,sexo = @sexo
                                where identidad = @identidad";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@identidad", paciente.Identidad);
                sqlCommand.Parameters.AddWithValue("@nombres", paciente.Nombres);
                sqlCommand.Parameters.AddWithValue("@apellidos", paciente.Apellidos);
                sqlCommand.Parameters.AddWithValue("@direccion", paciente.Direccion);
                sqlCommand.Parameters.AddWithValue("@correoElectronico", paciente.Correo);
                sqlCommand.Parameters.AddWithValue("@celular", paciente.Celular);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", paciente.FechaNacimiento.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@sexo", ObtenerSexo(paciente.SexoPaciente));

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
        /// Modifica el estado de los datos de un paciente.
        /// </summary>
        /// <param name="identidad">Información del paciente a eliminar</param>
        public void EliminarPaciente(string identidad)
        {
            try
            {
                // Query de eliminación
                string query = @"DELETE FROM [Pacientes].[Paciente]
                                 WHERE identidad = @identidad";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@identidad", identidad);

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
        /// Buscar una persona por su identidad
        /// </summary>
        /// <param name="identidad">Identidad del paciente a buscar</param>
        /// <returns>Paciente si es encontrado</returns>
        public Paciente BuscarPersona(string identidad)
        {
            Paciente elpaciente = new Paciente();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM [Pacientes].[Paciente]
                                 WHERE identidad = @identidad";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer el valor del parámetro
                sqlCommand.Parameters.AddWithValue("@identidad", identidad);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        elpaciente.Identidad = rdr["identidad"].ToString();
                        elpaciente.Nombres = rdr["nombres"].ToString();
                        elpaciente.Apellidos = rdr["apellidos"].ToString();
                        elpaciente.Correo = rdr["correoElectronico"].ToString();
                        elpaciente.Celular = rdr["celular"].ToString();
                        elpaciente.Direccion = rdr["direccion"].ToString();
                        elpaciente.FechaNacimiento = Convert.ToDateTime(rdr["fechaNacimiento"]);
                        elpaciente.SexoPaciente = (Sexo)Convert.ToChar(rdr["sexo"].ToString().Substring(0, 1));
                    }
                }

                return elpaciente;
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
