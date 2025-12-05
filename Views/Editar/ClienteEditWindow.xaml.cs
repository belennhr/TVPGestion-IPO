using System.Windows;

namespace TVPGestion_IPO.Views
{
    public partial class ClienteEditWindow : Window
    {
        public ClienteEditWindow(ClienteViewModel cliente)
        {
            InitializeComponent();
            this.DataContext = cliente;
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
