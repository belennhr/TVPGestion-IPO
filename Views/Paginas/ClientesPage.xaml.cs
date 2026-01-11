using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TVPGestion_IPO.Services;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public partial class ClientesPage : Page
    {
        private ObservableCollection<ClienteViewModel> clientesVM;
        private ICollectionView clientesView;
        private readonly ClienteService clienteService;

        public ClientesPage()
        {
            InitializeComponent();

            // 1. CARGAR CLIENTES
            // Según tus errores, tu servicio ya devuelve ClienteViewModel, así que no convertimos.
            clienteService = new ClienteService();
            var clientesCargados = clienteService.CargarClientes();

            // Asignamos directamente
            clientesVM = new ObservableCollection<ClienteViewModel>(clientesCargados);

            // 2. CARGAR PEDIDOS Y VINCULARLOS
            var pedidoService = new PedidoService();
            var todosLosPedidos = pedidoService.CargarPedidos();

            foreach (var clienteVM in clientesVM)
            {
                // Inicializamos la lista por si acaso viene nula del servicio
                if (clienteVM.HistorialPedidos == null)
                    clienteVM.HistorialPedidos = new ObservableCollection<PedidoResumenViewModel>();

                var susPedidos = todosLosPedidos
                                 .Where(p => clienteVM.EmailsString.Contains(p.ClienteEmail))
                                 .ToList();

                foreach (var p in susPedidos)
                {
                    // ARREGLO DE LA FECHA: Convertimos el string a DateTime antes de formatear
                    string fechaFormateada = p.FechaHoraRealizacion; // Valor por defecto
                    if (DateTime.TryParse(p.FechaHoraRealizacion, out DateTime fechaTemp))
                    {
                        fechaFormateada = fechaTemp.ToString("dd/MM/yyyy HH:mm");
                    }

                    clienteVM.HistorialPedidos.Add(new PedidoResumenViewModel
                    {
                        Id = p.Id,
                        Fecha = fechaFormateada, // Usamos la fecha corregida
                        Estado = p.Estado.ToString(),
                        ImporteTotal = p.ImporteTotal,
                        FormaPago = p.FormaPago
                    });
                }
            }

            // 3. VINCULAR A LA TABLA
            clientesView = CollectionViewSource.GetDefaultView(clientesVM);
            ClientesDataGrid.ItemsSource = clientesView;
        }

        private void GuardarCambios()
        {
            try
            {
                // Según el error, GuardarClientes espera una lista de ViewModels
                var listaParaGuardar = new List<ClienteViewModel>(clientesVM);
                clienteService.GuardarClientes(listaParaGuardar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        // --- El resto de métodos se mantienen igual ---
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = SearchBox.Text?.ToLower() ?? "";
            clientesView.Filter = item =>
            {
                var cli = item as ClienteViewModel;
                return cli != null && (
                    cli.Nombre.ToLower().Contains(filtro) ||
                    cli.Apellidos.ToLower().Contains(filtro) ||
                    cli.EmailsString.ToLower().Contains(filtro)
                );
            };
            clientesView.Refresh();
        }

        private void BtnAddCliente_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new ClienteAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                clientesVM.Add(addWindow.nuevoCliente);
                clientesView.Refresh();
                GuardarCambios();
            }
        }

        private void BtnDeleteCliente_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cliente = button?.DataContext as ClienteViewModel;
            if (cliente == null) return;

            if (MessageBox.Show($"¿Eliminar a {cliente.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                clientesVM.Remove(cliente);
                GuardarCambios();
            }
        }

        private void BtnEditCliente_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cliente = button?.DataContext as ClienteViewModel;
            if (cliente == null) return;

            var editWindow = new ClienteEditWindow(cliente)
            {
                Owner = Window.GetWindow(this)
            };

            if (editWindow.ShowDialog() == true)
            {
                clientesView.Refresh();
                GuardarCambios();
            }
        }
    }
}