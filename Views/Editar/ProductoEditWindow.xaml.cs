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
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para ProductoEditWindow.xaml
    /// </summary>
    public partial class ProductoEditWindow : Window
    {
        public ProductoEditWindow(ProductoViewModel producto)
        {
            InitializeComponent();
            this.DataContext = producto;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
