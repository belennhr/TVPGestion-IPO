using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ProductoCantidadViewModel
    {
        private string nombre;
        private decimal precio;
        private int cantidad;

        public string Nombre
        {
            get => nombre;
            set { nombre = value; }
        }

        public decimal Precio
        {
            get => precio;
            set { precio = value; }
        }

        public int Cantidad
        {
            get => cantidad;
            set { cantidad = value; }
        }
    }

    public class PedidoViewModel
    {
        public string Id { get; set; }
        public string FechaHoraRealizacion { get; set; }
        public string Medio { get; set; } // EnLocal, Telefono
        public string Modalidad { get; set; } // RecogerAhora, RecogerHora, Domicilio
        public string FechaHoraRecogida { get; set; }
        public string ClienteId { get; set; }
        public string ProductosString { get; set; } // Resumen: "Pizza x2, Refresco x1"
        public decimal ImporteTotal { get; set; }
        public string FormaPago { get; set; }
        public string Estado { get; set; } // EnElaboracion, Entregado, Recogido, Pagado, PendientePago
        public string DireccionEntrega { get; set; }
        public decimal CosteEnvio { get; set; }
        public bool EnvioGratisCanjeado { get; set; }
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
            return new PedidoViewModel
            {
                Id = pedido.Id,
                FechaHoraRealizacion = pedido.FechaHoraRealizacion.ToString("g"),
                Medio = pedido.Medio.ToString(),
                Modalidad = pedido.Modalidad.ToString(),
                FechaHoraRecogida = pedido.FechaHoraRecogida?.ToString("g") ?? "",
                ClienteId = pedido.ClienteId,
                ProductosString = string.Join(", ", pedido.Productos.Select(p => $"{p.Key.Nombre} x{p.Value}")),
                ImporteTotal = pedido.ImporteTotal,
                FormaPago = pedido.FormaPago,
                Estado = pedido.Estado.ToString(),
                DireccionEntrega = pedido.DireccionEntrega,
                CosteEnvio = pedido.CosteEnvio,
                EnvioGratisCanjeado = pedido.EnvioGratisCanjeado,
                PuntosGanados = CalcularPuntosGanados(pedido.ImporteTotal)
            };
        }

        // Método de conversión ViewModel → Model
        public Pedido ToPedido()
        {
            return new Pedido
            {
                Id = this.Id,
                FechaHoraRealizacion = DateTime.Parse(this.FechaHoraRealizacion),
                Medio = (MedioPedido)Enum.Parse(typeof(MedioPedido), this.Medio),
                Modalidad = (ModalidadEntrega)Enum.Parse(typeof(ModalidadEntrega), this.Modalidad),
                FechaHoraRecogida = string.IsNullOrEmpty(this.FechaHoraRecogida) ? (DateTime?)null : DateTime.Parse(this.FechaHoraRecogida),
                ClienteId = this.ClienteId,
                FormaPago = this.FormaPago,
                Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), this.Estado),
                DireccionEntrega = this.DireccionEntrega,
                CosteEnvio = this.CosteEnvio,
                EnvioGratisCanjeado = this.EnvioGratisCanjeado
            };
        }

        private static int CalcularPuntosGanados(decimal importe)
        {
            return (int)(importe / 10); // 1 punto por cada 10€
        }
    }
}