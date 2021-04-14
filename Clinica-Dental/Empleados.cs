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
    public enum Genero
    {
        Masculino = 'M',
        Femenino = 'F'
    }
    public enum EstadoEmpleado
    {
        Inactivo = 'I',
        Activo = 'A'
    }
    public enum Puesto
    {
        Recepcionista = 'R',
        Odontologo = 'O',
        HigienistaDental = 'H',
        Ortodontologo = 'G',
        AsistenteDental = 'A'
    }
    public class Empleados
    {
        // Variables miembro
        public  static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        public SqlConnection sqlConnection = new SqlConnection(connectionString);

        public string Identidad { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public Genero SexoEmpleado { get; set; }
        public EstadoEmpleado EstadoEmpleado { get; set; }
        public Puesto PuestoEmpleado { get; set; }

        public Empleados() { }
        public Empleados(string identidad, string nombres, string apellidos, string direccion, string correo, string celular, Genero sexo, Puesto puesto, EstadoEmpleado estado)
        {
            Identidad = identidad;
            Nombres = nombres;
            Apellidos = apellidos;
            Direccion = direccion;
            Correo = correo;
            Celular = celular;
            SexoEmpleado = sexo;
            EstadoEmpleado = estado;
            PuestoEmpleado = puesto;
        }

        private string ObtenerSexo(Genero sexo)
        {
            switch (sexo)
            {
                case Genero.Masculino:
                    return "Masculino";
                case Genero.Femenino:
                    return "Femenino";
                default:
                    return "Masculino";
            }
        }
        private string ObtenerEstado(EstadoEmpleado estado)
        {
            switch (estado)
            {
                case EstadoEmpleado.Activo:
                    return "Activo";
                case EstadoEmpleado.Inactivo:
                    return "Inactivo";
                default:
                    return "Activo";
            }
        }

        private string ObtenerPuesto(Puesto puesto)
        {
            switch (puesto)
            {
                case Puesto.Recepcionista:
                    return "Recepcionista";
                case Puesto.Ortodontologo:
                    return "Ortodontologo";
                case Puesto.Odontologo:
                    return "Odontologo";
                case Puesto.HigienistaDental:
                    return "Higienista Dental";
                case Puesto.AsistenteDental:
                    return "Asistente Dental";
                default:
                    return "Recepcionista";
            }
        }
        public void AgregarEmpleado(Empleados empleados)
        {
            try
            {
                // Query de inserción
                string query = @"insert into [Empleados].[Empleado]
                                values (@identidad,@nombres,@apellidos,@direccion,
                                @correo,@celular,@sexo, @estado, @puesto)";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros;
                sqlCommand.Parameters.AddWithValue("@identidad", empleados.Identidad);
                sqlCommand.Parameters.AddWithValue("@nombres", empleados.Nombres);
                sqlCommand.Parameters.AddWithValue("@apellidos", empleados.Apellidos);
                sqlCommand.Parameters.AddWithValue("@direccion", empleados.Direccion);
                sqlCommand.Parameters.AddWithValue("@correo", empleados.Correo);
                sqlCommand.Parameters.AddWithValue("@celular", empleados.Celular);
                sqlCommand.Parameters.AddWithValue("@sexo", ObtenerSexo(empleados.SexoEmpleado));
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(empleados.EstadoEmpleado));
                sqlCommand.Parameters.AddWithValue("@puesto", ObtenerPuesto(empleados.PuestoEmpleado));

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

        public List<Empleados> MostrarEmpleado()
        {
            //Inicializar una lista vacía de los empleados
            List<Empleados> empleado = new List<Empleados>();

            try
            {
                // Query de selección
                string query = @"select * from [Empleados].[Empleado]";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Obtener los datos de las empleados
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        empleado.Add(new Empleados
                        {
                            Identidad = rdr["identidad"].ToString(),
                            Nombres = rdr["nombres"].ToString(),
                            Apellidos = rdr["apellidos"].ToString(),
                            Correo = rdr["correoElectronico"].ToString(),
                            Celular = rdr["celular"].ToString(),
                            Direccion = rdr["direccion"].ToString(),
                            SexoEmpleado = (Genero)Convert.ToChar(rdr["sexo"].ToString().Substring(0, 1)),
                            EstadoEmpleado = (EstadoEmpleado)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1)),
                            PuestoEmpleado = (Puesto)Convert.ToChar(rdr["puesto"].ToString().Substring(0, 1))
                        }) ;
                    }
                }
                return empleado;
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

        public void ModificarEmpleado(Empleados empleados)
        {
            try
            {
                // Query de actualización
                string query = @"update [Empleados].[Empleado]
                                set  nombres = @nombres,apellidos = @apellidos,direccion = @direccion,
                                correoElectronico = @correoElectronico,celular = @celular,sexo = @sexo, 
                                estado = @estado, puesto = @puesto 
                                where identidad = @identidad";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@identidad", empleados.Identidad);
                sqlCommand.Parameters.AddWithValue("@nombres", empleados.Nombres);
                sqlCommand.Parameters.AddWithValue("@apellidos", empleados.Apellidos);
                sqlCommand.Parameters.AddWithValue("@direccion", empleados.Direccion);
                sqlCommand.Parameters.AddWithValue("@correoElectronico", empleados.Correo);
                sqlCommand.Parameters.AddWithValue("@celular", empleados.Celular);
                
                sqlCommand.Parameters.AddWithValue("@sexo", ObtenerSexo(empleados.SexoEmpleado));
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(empleados.EstadoEmpleado));
                sqlCommand.Parameters.AddWithValue("@puesto", ObtenerPuesto(empleados.PuestoEmpleado));

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

        public void EliminarEmpleado(string identidad)
        {
            try
            {
                // Query de eliminación
                string query = @"DELETE FROM [Empleados].[Empleado]
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

        public Empleados BuscarPersona(string identidad)
        {
            Empleados elempleado = new Empleados();

            try
            {
                // Query de búsqueda
                string query = @"SELECT * FROM [Empleados].[Empleado]
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
                        elempleado.Identidad = rdr["identidad"].ToString();
                        elempleado.Nombres = rdr["nombres"].ToString();
                        elempleado.Apellidos = rdr["apellidos"].ToString();
                        elempleado.Correo = rdr["correoElectronico"].ToString();
                        elempleado.Celular = rdr["celular"].ToString();
                        elempleado.Direccion = rdr["direccion"].ToString();
                        elempleado.SexoEmpleado = (Genero)Convert.ToChar(rdr["sexo"].ToString().Substring(0, 1));
                        elempleado.EstadoEmpleado = (EstadoEmpleado)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1));
                        elempleado.PuestoEmpleado = (Puesto)Convert.ToChar(rdr["puesto"].ToString().Substring(0, 1));
                    }
                }

                return elempleado;
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

