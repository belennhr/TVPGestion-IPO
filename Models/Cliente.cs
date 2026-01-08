using System;
using System.Collections.Generic;
using System.Linq;

namespace TVPGestion_IPO.Models
{
    public enum FormaPagoCliente
    {
        Efectivo,
        Tarjeta,
        Bizum,
        Transferencia
    }

    public class Cliente
    {
        public string Email { get; set; } // ID único del cliente
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public List<string> Direcciones { get; set; }
        public List<string> Telefonos { get; set; }
        public List<string> Emails { get; set; } // Emails adicionales
        public List<string> Alergias { get; set; }
        public FormaPagoCliente FormaPago { get; set; }
        public List<Pedido> HistorialPedidos { get; set; }
        public int PuntosAcumulados { get; set; }
        public int PuntosCanjeados { get; set; }
        public int PuntosDisponibles => PuntosAcumulados - PuntosCanjeados;

        public Cliente()
        {
            Direcciones = new List<string>();
            Telefonos = new List<string>();
            Emails = new List<string>();
            Alergias = new List<string>();
            HistorialPedidos = new List<Pedido>();
        }

        // Métodos de lógica de negocio
        public void AgregarPuntosPorPedido(decimal importeTotal, bool acumularPuntos)
        {
            if (acumularPuntos && importeTotal > 20)
            {
                PuntosAcumulados += 3;
            }
        }

        public bool CanjearPuntosEnvioGratis()
        {
            if (PuntosDisponibles >= 3)
            {
                PuntosCanjeados += 3;
                return true;
            }
            return false;
        }

        public void AgregarPedido(Pedido pedido)
        {
            if (!HistorialPedidos.Any(p => p.Id == pedido.Id))
            {
                HistorialPedidos.Add(pedido);
            }
        }
    }
}
