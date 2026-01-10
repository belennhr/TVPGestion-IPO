using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TVPGestion_IPO.Services;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Logica de interaccion para PedidoEditWindow.xaml
    /// </summary>
    public partial class PedidoEditWindow : Window
    {
        private PedidoViewModel pedidoViewModel;
        private ProductoService productoService;

        public PedidoEditWindow(PedidoViewModel pedido)
        {
            InitializeComponent();
            pedidoViewModel = pedido;
            productoService = new ProductoService();
            
            // Cargar productos disponibles
            CargarProductosDisponibles();
            
            this.DataContext = pedidoViewModel;
        }

        private void CargarProductosDisponibles()
        {
            // Limpiar la colección antes de cargar para evitar duplicados
            pedidoViewModel.ProductosDisponibles.Clear();
            
            var productos = productoService.CargarProductos();
            
            foreach (var prod in productos)
            {
                pedidoViewModel.ProductosDisponibles.Add(new ProductoCantidadViewModel
                {
                    Nombre = prod.Nombre,
                    Precio = decimal.Parse(prod.Precio),
                    Cantidad = 1
                });
            }
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

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProductos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un producto", "Producto no seleccionado", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Por favor, introduce una cantidad válida", "Cantidad inválida", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var productoSeleccionado = CmbProductos.SelectedItem as ProductoCantidadViewModel;
            
            // Verificar si el producto ya está en el pedido
            var productoExistente = pedidoViewModel.Productos.FirstOrDefault(p => p.Nombre == productoSeleccionado.Nombre);
            
            if (productoExistente != null)
            {
                // Si ya existe, incrementar la cantidad
                productoExistente.Cantidad += cantidad;
            }
            else
            {
                // Si no existe, agregarlo
                pedidoViewModel.Productos.Add(new ProductoCantidadViewModel
                {
                    Nombre = productoSeleccionado.Nombre,
                    Precio = productoSeleccionado.Precio,
                    Cantidad = cantidad
                });
            }

            // Limpiar cantidad
            TxtCantidad.Text = "1";
            
            // Actualizar importe total
            ActualizarImporteTotal();
        }

        private void BtnEliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                var producto = btn.Tag as ProductoCantidadViewModel;
                pedidoViewModel.Productos.Remove(producto);
                
                // Actualizar importe total
                ActualizarImporteTotal();
            }
        }

        private void TxtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo permite números enteros positivos
            Regex regex = new Regex(@"^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void ActualizarImporteTotal()
        {
            decimal total = 0;
            
            foreach (var producto in pedidoViewModel.Productos)
            {
                total += producto.Precio * producto.Cantidad;
            }
            
            // Agregar coste de envío si no está canjeado
            if (!pedidoViewModel.EnvioGratisCanjeado)
            {
                total += pedidoViewModel.CosteEnvio;
            }
            
            pedidoViewModel.ImporteTotal = total;
        }
    }
}
