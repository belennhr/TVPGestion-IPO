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
    /// Logica de interaccion para navBar.xaml
    /// </summary>
    public partial class navBar : Window
    {
        public navBar()
        {
            InitializeComponent();

            navframe.Navigate(new PedidosPage());
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // si nada seleccionado enseña ayuda por defecto


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
            else if (sidebar.SelectedIndex == 4)
            {
                // Editar Perfil
                EditarPerfilWindow editWindow = new EditarPerfilWindow();
                editWindow.ShowDialog();
                sidebar.SelectedIndex = -1;
            }
            else if (sidebar.SelectedIndex == 5)
            {
                // Ayuda
                navframe.Navigate(new Ayuda());

                sidebar.SelectedIndex = -1;
            }
            else if (sidebar.SelectedIndex == 3)
            {
                // Logout
                MessageBox.Show("Cerrando sesion...", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void btnEditPerfil_Click(object sender, RoutedEventArgs e)
        {
            EditarPerfilWindow editWindow = new EditarPerfilWindow(); 
            editWindow.Show();
            this.Close();
        }
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new Ayuda());
            sidebar.SelectedIndex = -1;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
        var result = MessageBox.Show(
        "¿Estás seguro de que deseas cerrar sesión?", "Confirmar cierre de sesión",
        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
        }

        private void btnFloatingHelp_Click(object sender, RoutedEventArgs e)
        {
            {
            if (HelpBubble.Visibility == Visibility.Visible)
            {
                HelpBubble.Visibility = Visibility.Collapsed;
            }
            else
            {
                HelpBubble.Visibility = Visibility.Visible;
            }
        }
        }

        // 2. Evento que detecta en qué página estás y cambia el texto
        private void navframe_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Opcional: Ocultar la ayuda automáticamente al cambiar de página
            HelpBubble.Visibility = Visibility.Collapsed;

            // Comprobamos qué página se ha cargado en el Frame
            if (navframe.Content is ProductosPage)
            {
                txtHelpContent.Text = "Estás en GESTIÓN DE PRODUCTOS.\n\n" +
                                      "• Usa el botón 'añadir producto' para añadir platos.\n" +
                                      "• Usa los iconos de lápiz para editar precios o alérgenos.\n" +
                                      "• Usa los iconos de basura para eliminar los productos";
            }
            else if (navframe.Content is ClientesPage)
            {
                txtHelpContent.Text = "Estás en GESTIÓN DE CLIENTES.\n\n" +
                                      "• Busca clientes por teléfono o nombre.\n" +
                                      "• Gestiona aquí sus puntos de fidelidad.";
            }
            else if (navframe.Content is PedidosPage)
            {
                txtHelpContent.Text = "Estás en PEDIDOS.\n\n" +
                                      "• Selecciona un cliente primero.\n" +
                                      "• Añade productos al carrito y finaliza la venta.";
            }
            else if (navframe.Content is EditarPerfilWindow) // O la página de perfil si es Page
            {
                txtHelpContent.Text = "Modifica aquí tu contraseña y datos de usuario.";
            }
            // Si tienes la página de 'Ayuda' (Info)
            else if (navframe.Content is Ayuda) 
            {
                txtHelpContent.Text = "Esta es la pantalla de información general del sistema y versión.";
            }
            else
            {
                // Texto por defecto si no reconoce la página
                txtHelpContent.Text = "Selecciona una opción del menú lateral para comenzar.";
            }
        }
    }
}
