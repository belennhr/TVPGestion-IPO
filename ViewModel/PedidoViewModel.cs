using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ProductoCantidadViewModel
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }

    public class PedidoViewModel
    {
        public string Id { get; set; }
        public string FechaHoraRealizacion { get; set; }
        public string Medio { get; set; } // EnLocal, Telefono
        public string Modalidad { get; set; } // RecogerAhora, RecogerHora, Domicilio
        public string FechaHoraRecogida { get; set; }
        public string ClienteEmail { get; set; } // Email del cliente (ID)
        public string ProductosString { get; set; } // Resumen: "Pizza x2, Refresco x1"
        public decimal ImporteTotal { get; set; }
        public string FormaPago { get; set; }
        public string Estado { get; set; } // EnElaboracion, Entregado, Recogido, Pagado, PendientePago
        public string DireccionEntrega { get; set; }
        public decimal CosteEnvio { get; set; }
        public bool EnvioGratisCanjeado { get; set; }
        public bool AcumularPuntos { get; set; } = true;
        public int PuntosGanados { get; set; }

        // Lista editable de productos y cantidades
        public ObservableCollection<ProductoCantidadViewModel> Productos { get; } = new ObservableCollection<ProductoCantidadViewModel>();

        // Listas estáticas para ComboBoxes
        public static List<string> MediosDisponibles => 
            Enum.GetNames(typeof(MedioPedido)).ToList();

        public static List<string> ModalidadesDisponibles => 
            Enum.GetNames(typeof(ModalidadEntrega)).ToList();

        public static List<string> EstadosDisponibles => 
            Enum.GetNames(typeof(EstadoPedido)).ToList();

        // Método de conversión Model → ViewModel
        public static PedidoViewModel FromPedido(Pedido pedido)
        {
            var vm = new PedidoViewModel
            {
                Id = pedido.Id,
                FechaHoraRealizacion = pedido.FechaHoraRealizacion.ToString("g"),
                Medio = pedido.Medio.ToString(),
                Modalidad = pedido.Modalidad.ToString(),
                FechaHoraRecogida = pedido.FechaHoraRecogida?.ToString("g") ?? "",
                ClienteEmail = pedido.ClienteEmail,
                ProductosString = string.Join(", ", pedido.Productos.Select(p => $"{p.Key.Nombre} x{p.Value}")),
                ImporteTotal = pedido.ImporteTotal,
                FormaPago = pedido.FormaPago,
                Estado = pedido.Estado.ToString(),
                DireccionEntrega = pedido.DireccionEntrega,
                CosteEnvio = pedido.CosteEnvio,
                EnvioGratisCanjeado = pedido.EnvioGratisCanjeado,
                AcumularPuntos = pedido.AcumularPuntos,
                PuntosGanados = pedido.CalcularPuntosGanados()
            };

            foreach (var prod in pedido.Productos)
            {
                vm.Productos.Add(new ProductoCantidadViewModel
                {
                    Nombre = prod.Key.Nombre,
                    Precio = prod.Key.Precio,
                    Cantidad = prod.Value
                });
            }

            return vm;
        }

        // Método de conversión ViewModel → Model (requiere catálogo de productos)
        public Pedido ToPedido(List<Producto> catalogoProductos)
        {
            var pedido = new Pedido
            {
                Id = this.Id,
                FechaHoraRealizacion = DateTime.Parse(this.FechaHoraRealizacion),
                Medio = (MedioPedido)Enum.Parse(typeof(MedioPedido), this.Medio),
                Modalidad = (ModalidadEntrega)Enum.Parse(typeof(ModalidadEntrega), this.Modalidad),
                FechaHoraRecogida = string.IsNullOrEmpty(this.FechaHoraRecogida) ? (DateTime?)null : DateTime.Parse(this.FechaHoraRecogida),
                ClienteEmail = this.ClienteEmail,
                FormaPago = this.FormaPago,
                Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), this.Estado),
                DireccionEntrega = this.DireccionEntrega,
                CosteEnvio = this.CosteEnvio,
                EnvioGratisCanjeado = this.EnvioGratisCanjeado,
                AcumularPuntos = this.AcumularPuntos
            };

            // Convertir productos
            foreach (var prodVM in this.Productos)
            {
                var producto = catalogoProductos.FirstOrDefault(p => p.Nombre == prodVM.Nombre);
                if (producto != null)
                {
                    pedido.Productos[producto] = prodVM.Cantidad;
                }
            }

            return pedido;
        }
    }
}