﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinica_Dental
{
    /// <summary>
    /// Lógica de interacción para detalleTratamientoU.xaml
    /// </summary>
    public partial class detalleTratamientoU : UserControl
    {
        // Variables miembro
        private DetalleTratamiento Tratamiento = new DetalleTratamiento();
        private List<DetalleTratamiento> detalleTratamientos;
        public detalleTratamientoU()
        {
            InitializeComponent();
            // Llenar el combobox de sexo
            cmbEstado.ItemsSource = Enum.GetValues(typeof(estado));
            // Llenar el listbox de DetalleTratamiento
            ObtenerDetalleTratamiento();
        }

        private void LimpiarFormulario()
        {
            txtNombreTratamiento.Text = string.Empty;
            txtDuracionTratamiento.Text = string.Empty;
            txtIndicacionesTratamiento.Text = string.Empty;
            txtPrecioTratamiento.Text = string.Empty;
            cmbEstado.SelectedValue = null;
        }
        private void ObtenerDetalleTratamiento()
        {
            detalleTratamientos = Tratamiento.MostrarDetalleTratamiento();
            dgvDetalleTratamiento.ItemsSource = detalleTratamientos;

        }
        private void ObtenerValoresFormulario()
        {
            Tratamiento.NombreTratamiento = txtNombreTratamiento.Text;
            Tratamiento.DuracionTratamiento = txtDuracionTratamiento.Text;
            Tratamiento.Precio = Convert.ToDecimal(txtPrecioTratamiento.Text);
            Tratamiento.Indicaciones = txtIndicacionesTratamiento.Text;
            Tratamiento.Estado = (estado)cmbEstado.SelectedItem;
        }
        private bool VerificarValores()
        {
            if (!EsEspacio(txtNombreTratamiento.Text))
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado del DetalleTratamiento");
                return false;
            }
            else if (!EsEspacio(txtDuracionTratamiento.Text))
            {
                MessageBox.Show("Por favor llena el campo de duracion.");
                return false;
            }
            else if (!EsEspacio(txtIndicacionesTratamiento.Text))
            {
                MessageBox.Show("Por favor llena el campo de indicaciones.");
                return false;
            }
            else if (!EsEspacio(txtNombreTratamiento.Text))
            {
                MessageBox.Show("Por favor llena el campo de nombre.");
                return false;
            }
            else if (!EsEspacio(txtPrecioTratamiento.Text))
            {
                MessageBox.Show("Por favor llena el campo de precio.");
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
            txtNombreTratamiento.Text = Tratamiento.NombreTratamiento;
            txtDuracionTratamiento.Text = Tratamiento.DuracionTratamiento;
            txtIndicacionesTratamiento.Text = Tratamiento.Indicaciones;
            txtPrecioTratamiento.Text = Convert.ToDecimal(Tratamiento.Precio).ToString();
            cmbEstado.SelectedValue = Tratamiento.Estado;

        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se ingresaron los valores requeridos
            if (VerificarValores())
            {
                try
                {
                    // Obtener los valores el detalleTratamiento
                    ObtenerValoresFormulario();

                    // Insertar los datos de el detalleTratamiento
                    Tratamiento.CrearDetalleTratamiento(Tratamiento);

                    // Mensaje de inserción exitosa
                    MessageBox.Show("¡Datos insertados correctamente!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de insertar el detalleTratamiento...");
                    Console.WriteLine(ex.Message);
                    
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerDetalleTratamiento();
                }
            }
        }
        

        private void dgvDetalleTratamiento_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetalleTratamiento tratamientoSelecionado = dgvDetalleTratamiento.SelectedItem as DetalleTratamiento;
            Tratamiento = Tratamiento.BuscarDetalleTratamiento(tratamientoSelecionado.IdTratamiento);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvDetalleTratamiento.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un tratamiento desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para el tratamiento desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar el tratamiento?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar Tratamiento
                            Tratamiento.ModificarDetalleTratamiento(Tratamiento);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Tratamiento modificado correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar el tratamiento...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los pacientes
                        ObtenerDetalleTratamiento();

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
                if (dgvDetalleTratamiento.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un tratamiento desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar el tratamiento?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar la habitación
                        Tratamiento.EliminarDetalleTratamiento(Tratamiento);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de eliminar el tratamiento...");
                Console.WriteLine(ex.Message);
                
            }
            finally
            {
                // Actualizar los pacientes
                ObtenerDetalleTratamiento();

                LimpiarFormulario();
                Habilitar();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Tratamiento = Tratamiento.BuscarDetalleTratamiento(Convert.ToInt32(txtDetalleTratamiento.Text));

            ValoresFormularioDesdeObjeto();
        }

        public bool EsEspacio(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
