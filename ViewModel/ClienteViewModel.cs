using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    public class ClienteViewModel
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DireccionesString { get; set; }
        public string TelefonosString { get; set; }
        public string EmailsString { get; set; }
        public string AlergiasString { get; set; }
        public string FormaPago { get; set; }
        public int PuntosAcumulados { get; set; }

        // Lista estática para ComboBox
        public static List<string> FormasPagoDisponibles => 
            Enum.GetNames(typeof(FormaPagoCliente)).ToList();
    }
}