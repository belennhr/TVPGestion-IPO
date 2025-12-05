using System.Windows;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Logica de interaccion para PedidoEditWindow.xaml
    /// </summary>
    public partial class PedidoEditWindow : Window
    {
        public PedidoEditWindow(PedidoViewModel pedido)
        {
            InitializeComponent();
            this.DataContext = pedido;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Los cambios han sido guardados correctamente", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
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
