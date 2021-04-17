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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinica_Dental
{
    /// <summary>
    /// Interaction logic for ListaCitas.xaml
    /// </summary>
    public partial class ListaCitas : UserControl
    {
        private ListaCita lista = new ListaCita();
        private List<ListaCita> listas;

        public ListaCitas()
        {
            InitializeComponent();

            dtpFechaCita.SelectedDate = DateTime.Now;

            ObtenerCitas();
        }

        private void ObtenerCitas()
        {
            listas = lista.MostrarLista(dtpFechaCita.SelectedDate.Value);
            dgvCitas.ItemsSource = listas;
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            ObtenerCitas();
        }
    }
}
