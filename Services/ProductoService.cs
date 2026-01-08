using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class ProductoService
    {
        private readonly DataPersistenceService<ProductoViewModel> persistenceService;

        public ProductoService()
        {
            persistenceService = new DataPersistenceService<ProductoViewModel>("productos.txt");
        }

        public List<ProductoViewModel> CargarProductos()
        {
            return persistenceService.LoadData();
        }

        public void GuardarProductos(List<ProductoViewModel> productos)
        {
            persistenceService.SaveData(productos);
        }

        public void AgregarProducto(ProductoViewModel producto)
        {
            persistenceService.AddItem(producto);
        }

        public void ActualizarProducto(string nombre, ProductoViewModel productoActualizado)
        {
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