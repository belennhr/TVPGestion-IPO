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
        public DateTime? FechaHoraRecogida { get; set; } // hora concreta a recoger en Establecimmiento o
                                                         // la hora a recibir a domicilio
        public string ClienteId { get; set; }
        public Dictionary<Producto, int> Productos { get; set; } // Producto y cantidad
        public decimal ImporteTotal => CalcularTotal();
        public string FormaPago { get; set; }
        public EstadoPedido Estado { get; set; }
        //A domicilio

        public string DireccionEntrega { get; set; } // Solo si RecogerEstablecimiento es false
        public decimal CosteEnvio { get; set; }
        public bool EnvioGratisCanjeado { get; set; } = false;
        private decimal CalcularTotal()
        {
            // Calcula el total sumando el precio de cada producto por su cantidad
            decimal total = 0;
            foreach (var item in Productos)
            {
                total += item.Key.Precio * item.Value;
            }
            total += CosteEnvio;
            return total;
        }
    }


}
