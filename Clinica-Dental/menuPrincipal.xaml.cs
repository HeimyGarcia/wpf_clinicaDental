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
using System.Windows.Threading;

namespace Clinica_Dental
{
    /// <summary>
    /// Lógica de interacción para menuPrincipal.xaml
    /// </summary>
    public partial class menuPrincipal : Window
    {
        DispatcherTimer Timer;
        double panelWidth;
        bool hidden;
        public menuPrincipal()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            panelWidth = SidePanel.Width;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                SidePanel.Width += 1;
                if (SidePanel.Width >= panelWidth)
                {
                    Timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                SidePanel.Width -= 1;
                if (SidePanel.Width <= 30)
                {
                    Timer.Stop();
                    hidden = true;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timer.Start();
            Close();
        }
        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListaView.SelectedIndex;
            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new empleadoU());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new usuarioU());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new detalleTratamientoU());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pacienteU());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new ListaCitas());
                    break;

                default:
                    break;
            }
        }

        
    }
}
