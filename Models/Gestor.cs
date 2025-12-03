using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVPGestion_IPO.Models
{
    public class Gestor
    {
        public string GestorId { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string FotoPerfil { get; set; }
        public DateTime FechaHoraUltimoAcceso { get; set; }
    }
}
