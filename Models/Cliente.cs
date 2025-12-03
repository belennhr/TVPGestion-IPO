using System;
using System.Collections.Generic;

namespace TVPGestion_IPO.Models
{
    public class Cliente
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public List<string> Direcciones { get; set; }
        public List<string> Telefonos { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Alergias { get; set; }
        public string FormaPago { get; set; }
        public List<Pedido> HistorialPedidos { get; set; }
        public int PuntosAcumulados { get; set; }
        public int PuntosCanjeados { get; set; }
    }
}
