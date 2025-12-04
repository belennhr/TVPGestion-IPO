using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    /// <summary>
    /// Lógica de interacción para ProductosPage.xaml
    /// </summary>
    public partial class ProductosPage : Page
    {
        private ObservableCollection<ProductoViewModel> productosVM;
        private ICollectionView productosView;

        public ProductosPage()
        {
            InitializeComponent();

            // Ejemplo de datos
            productosVM = new ObservableCollection<ProductoViewModel>
            {
                new ProductoViewModel { Nombre = "Pizza Margarita", Categoria = "Pizza", Subcategoria = "Clásica", Foto = "/Assets/Icons/comidaIcon.png", Precio = 8.99m, AlergenosString = "Gluten, Lácteos", IngredientesString = "Tomate, Queso, Albahaca" },
                new ProductoViewModel { Nombre = "Hamburguesa", Categoria = "Hamburguesa", Subcategoria = "Especial", Foto = "/Assets/Icons/comidaIcon.png", Precio = 6.99m, AlergenosString = "Gluten", IngredientesString = "Carne, Queso, Pan" },
                // ... más productos
            };

            productosView = CollectionViewSource.GetDefaultView(productosVM);
            ProductosDataGrid.ItemsSource = productosView;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = SearchBox.Text?.ToLower() ?? "";
            productosView.Filter = item =>
            {
                var prod = item as ProductoViewModel;
                return prod != null && (
                    prod.Nombre.ToLower().Contains(filtro) ||
                    prod.Categoria.ToLower().Contains(filtro) ||
                    prod.Subcategoria.ToLower().Contains(filtro) ||
                    prod.AlergenosString.ToLower().Contains(filtro) ||
                    prod.IngredientesString.ToLower().Contains(filtro)
                );
            };
            productosView.Refresh();
        }

        private void BtnAddProducto_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void BtnDeleteProducto_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el producto seleccionado
            var button = sender as Button;
            var producto = button?.DataContext as ProductoViewModel;
            if (producto == null) return;

            var result = MessageBox.Show(
                $"¿Estás seguro de que quieres eliminar el producto \"{producto.Nombre}\"?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                productosVM.Remove(producto);
                productosView.Refresh();
            }
        }
    }
}
