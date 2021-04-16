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

namespace Clinica_Dental
{
    /// <summary>
    /// Interaction logic for Pacientes.xaml
    /// </summary>
    public partial class Pacientes : Window
    {
        //Varieables miembro
        private Paciente paciente = new Paciente();
        private List<Paciente> pacientes;
        public Pacientes()
        {
            InitializeComponent();
            // Llenar el combobox de sexo
            cmbSexo.ItemsSource = Enum.GetValues(typeof(Sexo));

            // Llenar el combobox de estado
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadoPaciente));

            ObtenerPacientes();
        }

        private void ObtenerPacientes()
        {
            pacientes = paciente.MostrarPaciente();
            dgvPacientes.ItemsSource = pacientes;
        }

        private void LimpiarFormulario()
        {
            txtIdentidad.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtCelular.Text = string.Empty;
            dtpFechaNacimiento.SelectedDate = null;
            cmbSexo.SelectedValue = null;
            cmbEstado.SelectedValue = null;
        }

        private void ObtenerValoresFormulario()
        {
            paciente.Identidad = txtIdentidad.Text;
            paciente.Nombres = txtNombre.Text;
            paciente.Apellidos = txtApellido.Text;
            paciente.Direccion = txtDireccion.Text;
            paciente.Correo = txtCorreo.Text;
            paciente.Celular = txtCelular.Text;
            paciente.FechaNacimiento = Convert.ToDateTime(dtpFechaNacimiento.Text);
            paciente.SexoPaciente = (Sexo)cmbSexo.SelectedValue;
            paciente.Estado = (EstadoPaciente)cmbEstado.SelectedValue;

        }

        private void Inhabilitar()
        {
            txtIdentidad.IsEnabled = false;
            btnAgregar.IsEnabled = false;
        }
        private void Habilitar()
        {
            txtIdentidad.IsEnabled = true;
            btnAgregar.IsEnabled = true;
        }
        private void ValoresFormularioDesdeObjeto()
        {
            txtIdentidad.Text = paciente.Identidad;
            txtNombre.Text = paciente.Nombres;
            txtApellido.Text = paciente.Apellidos;
            txtDireccion.Text = paciente.Direccion;
            txtCorreo.Text = paciente.Correo;
            txtCelular.Text = paciente.Celular;
            dtpFechaNacimiento.SelectedDate = paciente.FechaNacimiento;
            cmbSexo.SelectedValue = paciente.SexoPaciente;
            cmbEstado.SelectedValue = paciente.Estado;
        }

        private bool VerificarValores()
        {
            if (txtIdentidad.Text == string.Empty || txtNombre.Text == string.Empty ||
                txtApellido.Text == string.Empty || txtDireccion.Text == string.Empty ||
                txtCorreo.Text == string.Empty || txtCelular.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbSexo.SelectedValue == null || cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado y sexo del paciente");
                return false;
            }
            else if (dtpFechaNacimiento.SelectedDate == null)
            {
                MessageBox.Show("Por favor selecciona la fecha de nacimiento");
                return false;
            }

            return true;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se ingresaron los valores requeridos
            if (VerificarValores())
            {
                try
                {
                    // Obtener los valores para el paciente
                    ObtenerValoresFormulario();

                    // Insertar los datos del paciente
                    paciente.AgregarPaciente(paciente);

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
                    ObtenerPacientes();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dgvPacientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Paciente pacienteSelecionado = dgvPacientes.SelectedItem as Paciente;
            paciente = paciente.BuscarPersona(pacienteSelecionado.Identidad);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvPacientes.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un paciente desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para del paciente desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar el paciente?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar Paciente
                            paciente.ModificarPaciente(paciente);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Paciente modificado correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar al paciente...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los pacientes
                        ObtenerPacientes();

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
                if (dgvPacientes.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un paciente desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar al paciente?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar el paciente
                        paciente.EliminarPaciente(paciente);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de eliminar al paciente...");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Actualizar los pacientes
                ObtenerPacientes();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            paciente = paciente.BuscarPersona(txtPaciente.Text);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnHistorial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del paciente desde la clase Paciente
                Paciente elPaciente = paciente.BuscarPersona(txtIdentidad.Text);

                // Verificar si el paciente existe
                if (elPaciente.Identidad == null)
                    MessageBox.Show("El paciente no ha sido seleccionado. Favor verificar.");
                else
                {
                    historialClinico elhistorialclinico = new historialClinico(elPaciente.Identidad);

                    elhistorialclinico.Show();

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
