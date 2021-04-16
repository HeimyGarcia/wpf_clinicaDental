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
    /// Lógica de interacción para usuario.xaml
    /// </summary>
    public partial class usuario : Window
    {
        private Usuarios usuarios = new Usuarios();
        private List<Usuarios> username;
        public usuario()
        {
            InitializeComponent();
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadoUsuario));
            cmbPuesto.ItemsSource = Enum.GetValues(typeof(TipoUsuario));
            ObtenerUsuario();
        }

        private void ObtenerUsuario()
        {
            username = usuarios.MostrarUsuario();
            dgvUsuario.ItemsSource = username;
        }

        private void LimpiarFormulario()
        {
            txtContraseña.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            cmbEstado.SelectedValue = null;
            cmbPuesto.SelectedValue = null;
        }

        private void ObtenerValoresFormulario()
        {
            usuarios.NombreCompleto = txtNombre.Text;
            usuarios.Username = txtUsuario.Text;
            usuarios.Password = txtContraseña.Text;
            usuarios.EstadoUsuario = (EstadoUsuario)cmbEstado.SelectedValue;
            usuarios.TipoUsuario = (TipoUsuario)cmbPuesto.SelectedValue;
        }
        private void ObtenerValoresDesdeObjeto()
        {
            txtNombre.Text = usuarios.NombreCompleto;
            txtUsuario.Text = usuarios.Username;
            txtContraseña.Text = usuarios.Password;
            cmbEstado.SelectedValue = usuarios.EstadoUsuario;
            cmbPuesto.SelectedValue = usuarios.TipoUsuario;
        }

        private bool VerificarDatos()
        {
            if (txtContraseña.Text == string.Empty || txtNombre.Text == string.Empty || txtUsuario.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado del usuario");
                return false;
            }
            else if (cmbPuesto.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el tipo de usuario");
                return false;
            }
            return true;
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            
            if (VerificarDatos())
            {
                try
                {
                   
                    ObtenerValoresFormulario();

                    
                    usuarios.AgregarUsuario(usuarios);

                    
                    MessageBox.Show("¡Datos insertados correctamente!");

                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerUsuario();
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void dgvUsuario_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Usuarios usuarioSeleccionado = dgvUsuario.SelectedItem as Usuarios;
            usuarios = usuarios.BuscarUsuario(usuarioSeleccionado.NombreCompleto);

            ObtenerValoresDesdeObjeto();
           
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsuario.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un usuario  desde el listado");
            else
            {
                if (VerificarDatos())
                {
                    try
                    {
                        ObtenerValoresFormulario();
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar el usuario?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar empleado
                            usuarios.ModificarUsuario(usuarios);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Usuario modificado correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Ha ocurrido un error al momento de modificar al usuario...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        ObtenerUsuario();

                        LimpiarFormulario();
                    }

                }
            }
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            usuarios = usuarios.BuscarUsuario(txtInfUsuario.Text);

            ObtenerValoresDesdeObjeto();
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvUsuario.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un usuario desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar al usuario?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar un empleado
                        usuarios.EliminarUsuario(txtNombre.Text);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ha ocurrido un error al momento de eliminar al usuario...");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ObtenerUsuario();

                LimpiarFormulario();
            }
        }
    }
}
