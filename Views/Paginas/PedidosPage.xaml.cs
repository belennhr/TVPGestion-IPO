using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TVPGestion_IPO.Services;

namespace TVPGestion_IPO.Views
{
    public partial class PedidosPage : Page
    {
        private ObservableCollection<PedidoViewModel> pedidosVM;
        private ICollectionView pedidosView;
        private readonly PedidoService pedidoService;

        public PedidosPage()
        {
            InitializeComponent();

            pedidoService = new PedidoService();

            // Cargar datos desde archivo
            var pedidosCargados = pedidoService.CargarPedidos();
            pedidosVM = new ObservableCollection<PedidoViewModel>(pedidosCargados);

            pedidosView = CollectionViewSource.GetDefaultView(pedidosVM);
            PedidosDataGrid.ItemsSource = pedidosView;
        }

        private void GuardarCambios()
        {
            try
            {
                var listaPedidos = new List<PedidoViewModel>(pedidosVM);
                pedidoService.GuardarPedidos(listaPedidos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar pedidos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = SearchBox.Text?.ToLower() ?? "";
            pedidosView.Filter = item =>
            {
                var ped = item as PedidoViewModel;
                return ped != null && (
                    ped.Id.ToLower().Contains(filtro) ||
                    ped.ClienteEmail.ToLower().Contains(filtro) ||
                    ped.ProductosString.ToLower().Contains(filtro) ||
                    ped.Estado.ToLower().Contains(filtro) ||
                    ped.ImporteTotal.ToString().Contains(filtro)
                );
            };
            pedidosView.Refresh();
        }

        private void BtnEditPedido_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pedido = button?.DataContext as PedidoViewModel;
            if (pedido == null) return;

            var editWindow = new PedidoEditWindow(pedido)
            {
                Owner = Window.GetWindow(this)
            };

            if (editWindow.ShowDialog() == true)
            {
                pedidosView.Refresh();
                GuardarCambios();
            }
        }

        private void BtnDeletePedido_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pedido = button?.DataContext as PedidoViewModel;
            if (pedido == null) return;

            var result = MessageBox.Show(
                $"¿Estás seguro de que quieres eliminar el pedido {pedido.Id}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                pedidosVM.Remove(pedido);
                pedidosView.Refresh();
                GuardarCambios();
            }
        }
    }
}
