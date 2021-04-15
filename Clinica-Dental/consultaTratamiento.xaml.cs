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
    /// Lógica de interacción para consultaTratamiento.xaml
    /// </summary>
    public partial class consultaTratamiento : Window
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        // Variables miembro
        private ConsultaTratamiento elTratamiento = new ConsultaTratamiento();
        private List<ConsultaTratamiento> tratamientos;

        //informacion de la llaves foraneas

        private DetalleTratamiento detalleTratamiento = new DetalleTratamiento();
        private List<DetalleTratamiento> detalleTratamientos = new List<DetalleTratamiento>();
        public consultaTratamiento(int idHistorialConsulta)
        {
            InitializeComponent();
            // Llenar el combobox de estado
            cmbEstado.ItemsSource = Enum.GetValues(typeof(estadoConsultaTratamiento));
            txtDetalleConsulta.Text =Convert.ToInt32( idHistorialConsulta).ToString();
            // Llenar el listbox de Tratamiento
            ObtenerTratamiento();
            //Llenar el combobox de detalleTratamiento
            ObtenerListaDetalleTratamiento();
        }

        private void ObtenerListaDetalleTratamiento()
        {
            SqlCommand command = new SqlCommand("Select nombreTratamiento, idTratamiento from Pacientes.DetalleTratamiento", sqlConnection);
            sqlConnection.Open();
            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                cmbTratamiento.Items.Add(rdr["idTratamiento"].ToString());
            }
            sqlConnection.Close();


        }
        private void LimpiarFormulario()
        {
            cmbEstado.Text = null;
            cmbTratamiento.Text = null;
            txtDetalleConsulta.Text = string.Empty;
        }
        private void ObtenerTratamiento()
        {
            tratamientos = elTratamiento.MostrarTratamiento();
            dgvTratamiento.ItemsSource = tratamientos;

        }

        private void ObtenerValoresFormulario()
        {
            elTratamiento.IdTratamiento = Convert.ToInt32(cmbTratamiento.Text);
            elTratamiento.IdHistorialConsulta = Convert.ToInt32(txtDetalleConsulta.Text);
            elTratamiento.Estado = (estadoConsultaTratamiento)cmbEstado.SelectedItem;
        }
        private bool VerificarValores()
        {
            if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado del DetalleTratamiento");
                return false;
            }
            else if (cmbTratamiento.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el tratamiento");
                return false;
            }
            else if (txtDetalleConsulta.Text == string.Empty)
            {
                MessageBox.Show("Por favor selecciona la consulta");
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
            cmbTratamiento.Text = Convert.ToInt32(elTratamiento.IdTratamiento).ToString();
            txtDetalleConsulta.Text = Convert.ToInt32(elTratamiento.IdHistorialConsulta).ToString();
            cmbEstado.SelectedValue = elTratamiento.Estado;

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
                    elTratamiento.CrearTratamiento(elTratamiento);

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
                    ObtenerTratamiento();
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dgvDetalleTratamiento_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ConsultaTratamiento tratamientoSelecionado = dgvTratamiento.SelectedItem as ConsultaTratamiento;
            elTratamiento = elTratamiento.BuscarTratamiento(tratamientoSelecionado.IdHistorialConsulta, tratamientoSelecionado.IdTratamiento);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTratamiento.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un tratamiento desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para la consulta desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar el tratamiento?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar Tratamiento
                            elTratamiento.ModificarTratamiento(elTratamiento);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Tratamiento modificada correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar el tratamiento ...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los pacientes
                        ObtenerTratamiento();

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
                if (dgvTratamiento.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un tratmiento desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar un tratamiento?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar la habitación
                        elTratamiento.EliminarTratamiento(elTratamiento);
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
                ObtenerTratamiento();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            elTratamiento = elTratamiento.BuscarTratamiento(Convert.ToInt32(txtTratamiento.Text), Convert.ToInt32(txtDetalleConsulta.Text));

            ValoresFormularioDesdeObjeto();
        }

        private void btnDetalleTratamiento_Click(object sender, RoutedEventArgs e)
        {
            detalleTratamiento detalletratamiento = new detalleTratamiento();
            detalletratamiento.Show();
        }
    }
}

