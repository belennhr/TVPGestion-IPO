using System.Net.Configuration;
using System.Windows;

namespace TVPGestion_IPO.Views
{
    public partial class EditarPerfilWindow : Window
    {
        public EditarPerfilWindow()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Perfil actualizado correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            navBar mainWindow = new navBar();
            this.Close();
            mainWindow.Show();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            navBar mainWindow = new navBar();
            this.Close();
            mainWindow.Show();
        }
    }
}