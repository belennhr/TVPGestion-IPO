using System.Windows;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Logica de interaccion para EditarPerfilWindow.xaml
    /// </summary>
    public partial class EditarPerfilWindow : Window
    {
        public EditarPerfilWindow()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Perfil actualizado correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}