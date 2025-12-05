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

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para navBar.xaml
    /// </summary>
    public partial class navBar : Window
    {
        public navBar()
        {
            InitializeComponent();
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sidebar.SelectedIndex == 0)
            {
                // Productos
                navframe.Navigate(new ProductosPage());
            }
            else if (sidebar.SelectedIndex == 1)
            {
                //Pedidos
                navframe.Navigate(new PedidosPage());
            }
            else if (sidebar.SelectedIndex == 2)
            {
                // Clientes
                navframe.Navigate(new ClientesPage());
            }
        }
    }
}
