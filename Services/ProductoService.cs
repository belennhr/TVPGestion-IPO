using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class ProductoService
    {
        private readonly DataPersistenceService<Producto> persistenceService;

        public ProductoService()
        {
            persistenceService = new DataPersistenceService<Producto>("productos.txt");
        }

        public List<ProductoViewModel> CargarProductos()
        {
            var productosModel = persistenceService.LoadData();
            
            // Si no hay datos, inicializar con ejemplos
            if (productosModel.Count == 0)
            {
                InicializarProductosEjemplo();
                productosModel = persistenceService.LoadData();
            }
            
            // Convertir Model ? ViewModel
            return productosModel.Select(p => ProductoViewModel.FromProducto(p)).ToList();
        }

        public void GuardarProductos(List<ProductoViewModel> productosVM)
        {
            // Convertir ViewModel ? Model
            var productosModel = productosVM.Select(vm => vm.ToProducto()).ToList();
            persistenceService.SaveData(productosModel);
        }

        public void InicializarProductosEjemplo()
        {
            var productosEjemplo = new List<Producto>
            {
                new Producto 
                { 
                    Nombre = "Pizza Margarita", 
                    Categoria = CategoriaProducto.Plato, 
                    Subcategoria = "Clásica", 
                    Foto = "/Assets/Icons/comidaIcon.png", 
                    Precio = 8.99m, 
                    Alergenos = new List<string> { "Gluten", "Lácteos" }, 
                    Ingredientes = new List<string> { "Tomate", "Queso", "Albahaca" } 
                },
                new Producto 
                { 
                    Nombre = "Hamburguesa Clásica", 
                    Categoria = CategoriaProducto.Plato, 
                    Subcategoria = "Especial", 
                    Foto = "/Assets/Icons/comidaIcon.png", 
                    Precio = 6.99m, 
                    Alergenos = new List<string> { "Gluten" }, 
                    Ingredientes = new List<string> { "Carne", "Queso", "Pan" } 
                },
                new Producto 
                { 
                    Nombre = "Coca-Cola", 
                    Categoria = CategoriaProducto.Bebida, 
                    Subcategoria = "Refresco", 
                    Foto = "/Assets/Icons/comidaIcon.png", 
                    Precio = 2.50m, 
                    Alergenos = new List<string> { "Ninguno" }, 
                    Ingredientes = new List<string> { "Agua", "Azúcar", "Caramelo" } 
                }
            };

            persistenceService.SaveData(productosEjemplo);
        }

        public void AgregarProducto(ProductoViewModel productoVM)
        {
            var producto = productoVM.ToProducto();
            persistenceService.AddItem(producto);
        }

        public void ActualizarProducto(string nombre, ProductoViewModel productoVM)
        {
            var productoActualizado = productoVM.ToProducto();
            persistenceService.UpdateItem(
                p => p.Nombre == nombre,
                productoActualizado
            );
        }

        public void EliminarProducto(string nombre)
        {
            persistenceService.DeleteItem(p => p.Nombre == nombre);
        }
    }
}