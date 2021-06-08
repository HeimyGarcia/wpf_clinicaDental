using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para Empleado.xaml
    /// </summary>
    public partial class Empleado : Window
    {
        private Empleados empleado = new Empleados();
        private List<Empleados> empleados;
        public Empleado()
        {
            InitializeComponent();
            cmbSexo.ItemsSource = Enum.GetValues(typeof(Genero));
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadoEmpleado));
            cmbPuesto.ItemsSource = Enum.GetValues(typeof(Puesto));
            ObtenerEmpleado();
        }
        private void ObtenerEmpleado()
        {
            empleados = empleado.MostrarEmpleado();
            dgvEmpleado.ItemsSource = empleados;
        }

        private void LimpiarFormulario()
        {
            txtIdentidad.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtCelular.Text = string.Empty;
            cmbEstado.SelectedValue = null;
            cmbPuesto.SelectedValue = null;
            cmbSexo.SelectedValue = null;
        }

        private void ObtenerValoresFormulario()
        {
            empleado.Identidad = txtIdentidad.Text;
            empleado.Nombres = txtNombre.Text;
            empleado.Apellidos = txtApellido.Text;
            empleado.Direccion = txtDireccion.Text;
            empleado.Correo = txtCorreo.Text;
            empleado.Celular = txtCelular.Text;
            empleado.SexoEmpleado = (Genero)cmbSexo.SelectedValue;
            empleado.EstadoEmpleado = (EstadoEmpleado)cmbEstado.SelectedValue;
            empleado.PuestoEmpleado = (Puesto)cmbPuesto.SelectedValue;
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
            txtIdentidad.Text = empleado.Identidad;
            txtNombre.Text = empleado.Nombres;
            txtApellido.Text = empleado.Apellidos;
            txtDireccion.Text = empleado.Direccion;
            txtCorreo.Text = empleado.Correo;
            txtCelular.Text = empleado.Celular;
            cmbSexo.SelectedValue = empleado.SexoEmpleado;
            cmbEstado.SelectedValue = empleado.EstadoEmpleado;
            cmbPuesto.SelectedValue = empleado.PuestoEmpleado;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalizar el dominio
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examina la parte del dominio del correo electrónico y la normaliza.
                string DomainMapper(Match match)
                {
                    // Utilice la clase IdnMapping para convertir nombres de dominio Unicode.
                    var idn = new IdnMapping();

                    // Extraiga y procese el nombre de dominio (arroja ArgumentException en inválido)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool VerificarValores()
        {
            if (txtIdentidad.Text == string.Empty && txtNombre.Text == string.Empty &&
                    txtApellido.Text == string.Empty && txtDireccion.Text == string.Empty &&
                    txtCorreo.Text == string.Empty && txtCelular.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa los datos corresponientes en cada una de las cajas de texto");
                return false;
            }
            else if (txtIdentidad.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa el numero de identidad del empleado");
                return false;
            }
            else if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa el nombre del empleado");
                return false;
            }
            else if (txtApellido.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa el apellido del empleado");
                return false;
            }
            else if (txtDireccion.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa la direccion del empleado");
                return false;
            }
            else if (txtCorreo.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa el correo del empleado");
                return false;
            }
            else if (txtCelular.Text == string.Empty)
            {
                MessageBox.Show("Por ingresa el numero de celular del empleado");
                return false;
            }
            else if (IsValidEmail(txtCorreo.Text) == false)
            {
                MessageBox.Show("El correo ingresado no tiene el formato correcto!!");
                return false;
            }
            else if (cmbSexo.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el sexo");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado del empleado");
                return false;
            }
            else if (cmbPuesto.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el puesto del empleado");
                return false;
            }
            return true;
        }
        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;

        }
        private void txtApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;

        }
       
        private void txtIdentidad_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;

        }
        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;

        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se ingresaron los valores requeridos
            if (VerificarValores())
            {
                try
                {
                    // Obtener los valores para el empleado
                    ObtenerValoresFormulario();

                    // Insertar los datos del empleado
                    empleado.AgregarEmpleado(empleado);

                    // Mensaje de inserción exitosa
                    MessageBox.Show("¡Datos insertados correctamente!");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Ha ocurrido un error al momento de insertar al paciente...");
                    //Console.WriteLine(ex.Message)
                    MessageBox.Show("Estimado usuario ya existe un empleado con esa identidad");
                    //throw ex;
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerEmpleado();
                }
            }
        }
        
        private void dgvEmpleado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Empleados empleadoSeleccionado = dgvEmpleado.SelectedItem as Empleados;
            empleado = empleado.BuscarPersona(empleadoSeleccionado.Identidad);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvEmpleado.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un estado desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para la habitación desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar el empleado?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar empleado
                            empleado.ModificarEmpleado(empleado);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Empleado modificado correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar al empleado...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los empleados
                        ObtenerEmpleado();

                        LimpiarFormulario();
                        Habilitar();
                    }
                }
            }
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            empleado = empleado.BuscarPersona(txtIdentidadEmpleado.Text);

            ValoresFormularioDesdeObjeto();

            Inhabilitar();
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvEmpleado.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un paciente desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar al empleado?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar un empleado
                        empleado.EliminarEmpleado(empleado);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de eliminar al empleado...");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Actualizar los empleados
               ObtenerEmpleado();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
