using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        // Lista editable de productos y cantidades (para UI avanzada)
        public ObservableCollection<ProductoCantidadViewModel> Productos { get; } = new ObservableCollection<ProductoCantidadViewModel>();
    }
}