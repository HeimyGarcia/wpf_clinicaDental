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
    class ListaCita
    {
        // Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public int IdCita { get; set; }
        public string Identidad { get; set; }
        public string Nota { get; set; }
        public string Paciente { get; set; }
        public TimeSpan Hora { get; set; }

        //Constructores
        public ListaCita() { }

        public ListaCita(int id, string identidad, string nota, string paciente, TimeSpan hora)
        {
            IdCita = id;
            Identidad = identidad;
            Nota = nota;
            Paciente = paciente;
            Hora = hora;

        }

        //Metodos
        public List<ListaCita> MostrarLista(DateTime fecha)
        {
            //Inicializar una lista vacía de los las citas
            List<ListaCita> listas = new List<ListaCita>();

            try
            {
                // Query de selección
                string query = @"select a.idCita as  ID, a.hora as Hora,a.nota as Nota, c.identidad as Identidad, c.nombres + ' ' + c.apellidos as Paciente
                                from [Pacientes].[Cita] a inner join [Pacientes].[HistorialClinico] b
                                on a.idHistorialClinico = b.idHistorialClinico inner join [Pacientes].[Paciente] c
                                on b.identidadPaciente = c.identidad
                                where a.fechaCita = @fecha
                                order by a.hora";

                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@fecha", fecha);

                // Obtener los datos de la citas
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        listas.Add(new ListaCita
                        {
                            IdCita = Convert.ToInt32(rdr["ID"]),
                            Identidad = rdr["Identidad"].ToString(),
                            Nota = rdr["Nota"].ToString(),
                            Paciente = rdr["Paciente"].ToString(),
                            Hora = TimeSpan.Parse((string)rdr["Hora"].ToString())
                        });
                    }
                }
                return listas;
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
