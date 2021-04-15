using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace Clinica_Dental
{
    /// <summary>
    /// Lógica de interacción para historialConsulta.xaml
    /// </summary>
    public partial class historialConsulta : Window
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Variables miembro
        private HistorialConsulta consulta = new HistorialConsulta();
        private List<HistorialConsulta> historialConsultas;

        //informacion foranea
        private Empleados empleado = new Empleados();
        private List<Empleados> empleados = new List<Empleados>();
        public historialConsulta(int idHistorialClinico)
        {
            InitializeComponent();
            // Llenar el combobox de estado
            cmbEstado.ItemsSource = Enum.GetValues(typeof(estadoHistorial));
            txtHistorialClinico.Text = Convert.ToInt32(idHistorialClinico).ToString();
            //Llenar el combobox de Empleado
            ObtenerListaEmpleado();
            // Llenar el listbox de DetalleTratamiento
            ObtenerHistorialConsulta();
            
        }
        private void ObtenerListaEmpleado()
        {
            SqlCommand command = new SqlCommand("Select identidad from Empleados.Empleado", sqlConnection);
            sqlConnection.Open();
            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                cmbEmpleado.Items.Add(rdr["identidad"].ToString());

            }
            sqlConnection.Close();

        }
        private void LimpiarFormulario()
        {
            txtObservaciones.Text = string.Empty;
            txtMotivoConsulta.Text = string.Empty;
            cmbEmpleado.Text = string.Empty;
            cmbEstado.Text = null;
            txtHistorialClinico.Text = string.Empty;
            txtConsulta.Text = string.Empty;
        }
        private void ObtenerHistorialConsulta()
        {
            historialConsultas = consulta.MostrarHistorialConsulta();
            dgvHistorialConsulta.ItemsSource = historialConsultas;

        }
        private void ObtenerValoresFormulario()
        {
            consulta.IdHistorialClinico = Convert.ToInt32(txtHistorialClinico.Text);
            consulta.FechaConsulta = DateTime.Now;
            consulta.MotivoConsulta = txtMotivoConsulta.Text;
            consulta.Observaciones = txtObservaciones.Text;
            consulta.IdentidadEmpleado = cmbEmpleado.SelectedItem.ToString();
            consulta.Estado = (estadoHistorial)cmbEstado.SelectedItem;
        }
        private bool VerificarValores()
        {
            if (txtMotivoConsulta.Text == string.Empty ||
                txtObservaciones.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado ");
                return false;
            }
            else if (cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el empleado");
                return false;
            }
            else if (txtHistorialClinico.Text == string.Empty)
            {
                MessageBox.Show("Por favor selecciona el HistorialClinico");
                return false;
            }

            return true;
        }
        private void Inhabilitar()
        {
            btnAgregar.IsEnabled = false;
        }
        private void Habilitar()
        {
            btnAgregar.IsEnabled = true;
        }
        private void ValoresFormularioDesdeObjeto()
        {
            txtObservaciones.Text = consulta.Observaciones;
            txtMotivoConsulta.Text = consulta.MotivoConsulta;
            txtHistorialClinico.Text = Convert.ToInt32(consulta.IdHistorialClinico).ToString();
            txtConsulta.Text = Convert.ToInt32(consulta.IdHistorialConsulta).ToString();
            cmbEmpleado.SelectedValue = consulta.IdentidadEmpleado;
            cmbEstado.SelectedValue = consulta.Estado;

        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se ingresaron los valores requeridos
            if (VerificarValores())
            {
                try
                {
                    // Obtener los valores para la habitación
                    ObtenerValoresFormulario();

                    // Insertar los datos de la habitación
                    consulta.CrearHistorialConsulta(consulta);

                    // Mensaje de inserción exitosa
                    MessageBox.Show("¡Datos insertados correctamente!");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Ha ocurrido un error al momento de insertar al paciente...");
                    //Console.WriteLine(ex.Message)
                    throw ex;
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerHistorialConsulta();
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dgvDetalleTratamiento_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HistorialConsulta tratamientoSelecionado = dgvHistorialConsulta.SelectedItem as HistorialConsulta;
            consulta = consulta.BuscarHistorialConsulta(tratamientoSelecionado.IdHistorialClinico);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvHistorialConsulta.SelectedValue == null)
                MessageBox.Show("Por favor selecciona una consulta desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para la consulta desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar la consulta?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar Tratamiento
                            consulta.ModificarHistorialConsulta(consulta);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Consulta modificada correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar la consulta ...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los pacientes
                        ObtenerHistorialConsulta();

                        LimpiarFormulario();
                        Habilitar();
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvHistorialConsulta.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona una consulta desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar una consulta?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar la habitación
                        consulta.EliminarHistorialConsulta(consulta);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ha ocurrido un error al momento de eliminar la consulta...");
                //Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                // Actualizar los pacientes
                ObtenerHistorialConsulta();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            consulta = consulta.BuscarHistorialConsulta(Convert.ToInt32(txtHistorialConsulta.Text));

            ValoresFormularioDesdeObjeto();
        }

        private void btnTratamiento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del historialconsulta desde la clase HistorialConsulta
                HistorialConsulta loshistoriales = consulta.BuscarHistorialConsulta2(Convert.ToInt32(txtConsulta.Text));

                // Verificar si el historial existe
                if (Convert.ToInt32(loshistoriales.IdHistorialConsulta).ToString() == null)
                    MessageBox.Show("El historialConsulta no ha sido seleccionado. Favor verificar.");
                else
                {
                    consultaTratamiento consultatratamiento = new consultaTratamiento(Convert.ToInt32(loshistoriales.IdHistorialConsulta));

                    consultatratamiento.Show();

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
