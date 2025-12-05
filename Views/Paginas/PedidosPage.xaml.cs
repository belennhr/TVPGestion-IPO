using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Logica de interaccion para PedidosPage.xaml
    /// </summary>
    public partial class PedidosPage : Page
    {
        private ObservableCollection<PedidoViewModel> pedidosVM;
        private ICollectionView pedidosView;

        public PedidosPage()
        {
            InitializeComponent();

            // Ejemplo de datos
            pedidosVM = new ObservableCollection<PedidoViewModel>
            {
                new PedidoViewModel { Id = "P001", FechaHoraRealizacion = "2024-01-15 14:30", Medio = "EnLocal", Modalidad = "RecogerAhora", ClienteId = "C001", ProductosString = "Pizza Margarita x2", ImporteTotal = 17.98m, FormaPago = "Tarjeta", Estado = "EnElaboracion", DireccionEntrega = "", CosteEnvio = 0m, PuntosGanados = 0 },
                new PedidoViewModel { Id = "P002", FechaHoraRealizacion = "2024-01-15 15:45", Medio = "Telefono", Modalidad = "Domicilio", ClienteId = "C002", ProductosString = "Hamburguesa x1, Refresco x1", ImporteTotal = 8.99m, FormaPago = "Efectivo", Estado = "PendientePago", DireccionEntrega = "Calle Falsa 123", CosteEnvio = 2.5m, PuntosGanados = 0 },
            };

            pedidosView = CollectionViewSource.GetDefaultView(pedidosVM);
            PedidosDataGrid.ItemsSource = pedidosView;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = SearchBox.Text?.ToLower() ?? "";
            pedidosView.Filter = item =>
            {
                var ped = item as PedidoViewModel;
                return ped != null && (
                    (ped.Id ?? "").ToLower().Contains(filtro) ||
                    (ped.ClienteId ?? "").ToLower().Contains(filtro) ||
                    (ped.ProductosString ?? "").ToLower().Contains(filtro) ||
                    (ped.Estado ?? "").ToLower().Contains(filtro) ||
                    (ped.FormaPago ?? "").ToLower().Contains(filtro) ||
                    (ped.Medio ?? "").ToLower().Contains(filtro) ||
                    (ped.Modalidad ?? "").ToLower().Contains(filtro)
                );
            };
            pedidosView.Refresh();
        }

        private void BtnAddPedido_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var nuevo = new PedidoViewModel
            {
                Id = "P" + (pedidosVM.Count + 1).ToString("D3"),
                FechaHoraRealizacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Medio = "EnLocal",
                Modalidad = "RecogerAhora",
                ClienteId = "",
                ProductosString = "",
                ImporteTotal = 0m,
                FormaPago = "",
                Estado = "EnElaboracion",
                DireccionEntrega = "",
                CosteEnvio = 0m,
                PuntosGanados = 0
            };

            var editWindow = new PedidoEditWindow(nuevo);
            if (editWindow.ShowDialog() == true)
            {
                pedidosVM.Add(nuevo);
                pedidosView.Refresh();
            }
        }

        private void BtnDeletePedido_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el pedido seleccionado
            var button = sender as Button;
            var pedido = button?.DataContext as PedidoViewModel;
            if (pedido == null) return;

            var result = MessageBox.Show(
                $"Estás seguro de que quieres eliminar el pedido \"{pedido.Id}\"?",
                "Confirmar eliminacion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                pedidosVM.Remove(pedido);
                pedidosView.Refresh();
            }
        }

        private void BtnEditPedido_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pedido = button?.DataContext as PedidoViewModel;
            if (pedido == null) return;

            var editWindow = new PedidoEditWindow(pedido);
            if (editWindow.ShowDialog() == true)
            {
                pedidosView.Refresh();
            }
        }
    }
}
