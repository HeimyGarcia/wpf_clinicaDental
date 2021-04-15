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
    /// Lógica de interacción para cita.xaml
    /// </summary>
    public partial class cita : Window
    {
        //Varieables miembro
        private Citas citas = new Citas();
        private List<Citas> lascitas;

        public cita()
        {
            InitializeComponent();

            // Llenar el combobox de estado
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadoCita));

            ObtenerCitas();
        }

        private void ObtenerCitas()
        {
            lascitas = citas.MostrarCitas();
            dgvCita.ItemsSource = lascitas;
        }

        private void LimpiarFormulario()
        {
            txtIdCita.Text = string.Empty;
            txtNota.Text = string.Empty;
            txtCitas.Text = string.Empty;
            dtpFechaCita.SelectedDate = null;
            tmpHora.SelectedTime = null;
            cmbEstado.SelectedValue = null;
        }

        private void ObtenerValoresFormulario()
        {
            citas.IdCita = Convert.ToInt32(txtIdCita.Text);
            citas.IdHistorialClinico = Convert.ToInt32(txtIdHistorialClinico.Text);
            citas.Nota = txtNota.Text;
            citas.FechaCita = dtpFechaCita.SelectedDate.Value;
            citas.Hora = tmpHora.SelectedTime.Value;
            citas.EstadoCita = (EstadoCita)cmbEstado.SelectedItem;

        }

        private void Inhabilitar()
        {
            txtIdCita.IsEnabled = false;
            btnAgregar.IsEnabled = false;
        }
        private void Habilitar()
        {
            txtIdCita.IsEnabled = true;
            btnAgregar.IsEnabled = true;
        }

        private void ValoresFormularioDesdeObjeto()
        {
            txtIdCita.Text = citas.IdCita.ToString();
            txtIdHistorialClinico.Text = citas.IdHistorialClinico.ToString();
            txtNota.Text = citas.Nota;
            dtpFechaCita.SelectedDate = citas.FechaCita;
            tmpHora.SelectedTime = citas.Hora;
            cmbEstado.SelectedValue = citas.EstadoCita;
        }

        private bool VerificarValores()
        {
            if (txtIdCita.Text == string.Empty || txtNota.Text == string.Empty ||
                txtIdHistorialClinico.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado y sexo de la cita");
                return false;
            }
            else if (dtpFechaCita.SelectedDate == null || tmpHora.SelectedTime == null)
            {
                MessageBox.Show("Por favor selecciona la fecha de la cita y la hora");
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
                    // Obtener los valores para la cita
                    ObtenerValoresFormulario();

                    // Insertar los datos de la cita
                    citas.AgregarCita(citas);

                    // Mensaje de inserción exitosa
                    MessageBox.Show("¡Datos insertados correctamente!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de insertar la cita...");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerCitas();
                }
            }
        }

        private void dgvPacientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Citas citaSelecionada = dgvCita.SelectedItem as Citas;
            citas = citas.BuscarCita(citaSelecionada.IdCita);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCita.SelectedValue == null)
                MessageBox.Show("Por favor selecciona una cita desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para de la cita desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar la cita?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar Cita
                            citas.ModificarCita(citas);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Cita modificada correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar la cita...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar las citas
                        ObtenerCitas();

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
                if (dgvCita.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona una cita desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar la cita?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar la cita
                        citas.EliminarCita(citas);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de eliminar la cita...");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Actualizar las citas
                ObtenerCitas();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            citas = citas.BuscarCita(Convert.ToInt32(txtCitas.Text));

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
