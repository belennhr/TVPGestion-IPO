using System;
using System.Collections.Generic;

namespace TVPGestion_IPO.Models
{
    public enum MedioPedido { EnLocal, Telefono }
    public enum ModalidadEntrega { RecogerAhora, RecogerHora, Domicilio }

    public enum EstadoPedido
    {
        EnElaboracion,
        Entregado,
        Recogido,
        Pagado,
        PendientePago
    }

    public class Pedido
    {
        //Campos comunes:
        public string Id { get; set; }
        public DateTime FechaHoraRealizacion { get; set; }
        public MedioPedido Medio { get; set; }
        public ModalidadEntrega Modalidad { get; set; }
        public DateTime? FechaHoraRecogida { get; set; } // hora concreta a recoger en Establecimiento o
                                                          // la hora a recibir a domicilio
        public string ClienteEmail { get; set; } // Email del cliente (ID)
        public Dictionary<Producto, int> Productos { get; set; } // Producto y cantidad
        public decimal ImporteTotal => CalcularTotal(); //incluido coste de envio
        public string FormaPago { get; set; }
        public EstadoPedido Estado { get; set; }

        //A domicilio
        public string DireccionEntrega { get; set; } // Solo si es a domicilio
        public decimal CosteEnvio { get; set; }
        public bool EnvioGratisCanjeado { get; set; } = false;
        public bool AcumularPuntos { get; set; } = true; // True = acumular, False = no acumular

        public Pedido()
        {
            Productos = new Dictionary<Producto, int>();
            FechaHoraRealizacion = DateTime.Now;
        }

        private decimal CalcularTotal()
        {
            // Calcula el total sumando el precio de cada producto por su cantidad
            decimal total = 0;
            foreach (var item in Productos)
            {
                total += item.Key.Precio * item.Value;
            }
            
            // Si el envío no fue canjeado, se suma al total
            if (!EnvioGratisCanjeado)
            {
                total += CosteEnvio;
            }
            
            return total;
        }

        public decimal CalcularImporteSinEnvio()
        {
            decimal total = 0;
            foreach (var item in Productos)
            {
                total += item.Key.Precio * item.Value;
            }
            return total;
        }

        public int CalcularPuntosGanados()
        {
            return CalcularImporteSinEnvio() > 20 ? 3 : 0;
        }
    }
}
