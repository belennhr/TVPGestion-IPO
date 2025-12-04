using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para ClientesPage.xaml
    /// </summary>
    public partial class ClientesPage : Page
    {
        private ObservableCollection<ClienteViewModel> clientesVM;
        private ICollectionView clientesView;

        public ClientesPage()
        {
            InitializeComponent();

            // Ejemplo de datos
            clientesVM = new ObservableCollection<ClienteViewModel>
            {
                new ClienteViewModel { Nombre = "Juan", Apellidos = "Pérez", DireccionesString = "Calle 1, Ciudad", TelefonosString = "123456789", EmailsString = "juan@mail.com", AlergiasString = "Ninguna", FormaPago = "Tarjeta", PuntosAcumulados = 100 },
                new ClienteViewModel { Nombre = "Ana", Apellidos = "García", DireccionesString = "Avenida 2, Ciudad", TelefonosString = "987654321", EmailsString = "ana@mail.com", AlergiasString = "Gluten", FormaPago = "Bizum", PuntosAcumulados = 50 }
                // ... más clientes
            };

            clientesView = CollectionViewSource.GetDefaultView(clientesVM);
            ClientesDataGrid.ItemsSource = clientesView;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = SearchBox.Text?.ToLower() ?? "";
            clientesView.Filter = item =>
            {
                var cli = item as ClienteViewModel;
                return cli != null && (
                    cli.Nombre.ToLower().Contains(filtro) ||
                    cli.Apellidos.ToLower().Contains(filtro) ||
                    cli.DireccionesString.ToLower().Contains(filtro) ||
                    cli.TelefonosString.ToLower().Contains(filtro) ||
                    cli.EmailsString.ToLower().Contains(filtro) ||
                    cli.AlergiasString.ToLower().Contains(filtro) ||
                    cli.FormaPago.ToLower().Contains(filtro) ||
                    cli.PuntosAcumulados.ToString().Contains(filtro)
                );
            };
            clientesView.Refresh();
        }

        private void BtnAddCliente_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
