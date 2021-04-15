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
    public enum EstadoUsuario
    {
        Activo = 'A',
        Inactivo = 'I'
    }

    public enum TipoUsuario
    {
        Administrador = 'A',
        Usuario = 'U'
    }

    public class Usuarios
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Propiedades
        public int Id { get; set; }

        public string NombreCompleto { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public EstadoUsuario EstadoUsuario { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        public Usuarios() { }
        public Usuarios(int id, string nombreCompleto, string username, string password, EstadoUsuario estado, TipoUsuario tipoUsuario)
        {
            Id = id;
            NombreCompleto = nombreCompleto;
            Username = username;
            Password = password;
            EstadoUsuario = estado;
            TipoUsuario = tipoUsuario;
        }

        private string ObtenerEstado(EstadoUsuario estadoUsuario)
        {
            switch (estadoUsuario)
            {
                case EstadoUsuario.Activo:
                    return "Activo";
                case EstadoUsuario.Inactivo:
                    return "Inactivo";
                default:
                    return "Activo";
            }
        }

        private string ObtenerTipoUsuario(TipoUsuario tipoUsuario)
        {
            switch (tipoUsuario)
            {
                case TipoUsuario.Administrador:
                    return "Administrador";
                case TipoUsuario.Usuario:
                    return "Usuario";
                default:
                    return "Administrador";
            }
        }

        public void AgregarUsuario(Usuarios usuarios)
        {
            try
            {
                string query = @"insert into [Empleados].[Usuario] (nombreCompleto, username, password, estado, tipoUsuario)
                            values (@)nombreCompleto, @username, @username, @estado, @tipoUsuario";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@nombreCompleto", usuarios.NombreCompleto);
                sqlCommand.Parameters.AddWithValue("@username", usuarios.Username);
                sqlCommand.Parameters.AddWithValue("@password", usuarios.Password);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(usuarios.EstadoUsuario));
                sqlCommand.Parameters.AddWithValue("@tipoUsuario", ObtenerTipoUsuario(usuarios.TipoUsuario));

                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public List<Usuarios> MostrarUsuario()
        {
            List<Usuarios> usuarios = new List<Usuarios>();

            try
            {
                string query = @"select * from [Empleados].[Usuario]";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        usuarios.Add(new Usuarios
                        {
                            NombreCompleto = rdr["nombreCompleto"].ToString(),
                            Username = rdr["username"].ToString(),
                            Password = rdr["password"].ToString(),
                            EstadoUsuario = (EstadoUsuario)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1)),
                            TipoUsuario = (TipoUsuario)Convert.ToChar(rdr["tipoUsuario"].ToString().Substring(0, 1))

                        });
                    }
                }
                return usuarios;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void ModificarUsuario(Usuarios usuarios)
        {
            try
            {
                string query = @"update [Empleados].[Usuario] set 
                            nombreCompleto = @nombreCompleto, username = @username, password = @password,
                            estado = @estado, tipoUsuario = @tipoUsuario where nombreCompleto = @nombreCompleto"
                            ;
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@nombreCompleto", usuarios.NombreCompleto);
                sqlCommand.Parameters.AddWithValue("@username", usuarios.Username);
                sqlCommand.Parameters.AddWithValue("@password", usuarios.Password);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(usuarios.EstadoUsuario));
                sqlCommand.Parameters.AddWithValue("@tipoUsuario", ObtenerTipoUsuario(usuarios.TipoUsuario));

                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void EliminarUsuario(string nombreCompleto)
        {
            try
            {
                string query = @"delete from [Empleados].[Usuarios] where nombreCompleto = @nombreCompleto";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public Usuarios BuscarUsuario(string nombreCompleto)
        {
            Usuarios usuarios = new Usuarios();
            try
            {
                string query = @"SELECT * FROM [Empleados].[Usuario]
                                 WHERE id = @id";

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        usuarios.NombreCompleto = rdr["nombreCompleto"].ToString();
                        usuarios.Username = rdr["username"].ToString();
                        usuarios.Password = rdr["password"].ToString();
                        usuarios.EstadoUsuario = (EstadoUsuario)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1));
                        usuarios.TipoUsuario = (TipoUsuario)Convert.ToChar(rdr["tipoUsuario"].ToString().Substring(0, 1));
                    }
                }
                return usuarios;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
