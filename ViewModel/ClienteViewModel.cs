using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Necesario para la lista dinámica
using System.Linq;
using TVPGestion_IPO.Models;

namespace TVPGestion_IPO.Views
{
    // Clase auxiliar para la fila de la tabla
    public class PedidoResumenViewModel
    {
        public string Id { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public decimal ImporteTotal { get; set; }
        public string FormaPago { get; set; }
    }

    public class ClienteViewModel
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DireccionesString { get; set; }
        public string TelefonosString { get; set; }
        public string EmailsString { get; set; }
        public string AlergiasString { get; set; }
        public string FormaPago { get; set; }
        public int PuntosAcumulados { get; set; }
        public int PuntosCanjeados { get; set; }
        public int PuntosDisponibles => PuntosAcumulados - PuntosCanjeados;
        public string HistorialPedidosIds { get; set; }

        // ESTA ES LA LISTA CLAVE: La iniciamos vacía y del tipo correcto
        public ObservableCollection<PedidoResumenViewModel> HistorialPedidos { get; set; }
            = new ObservableCollection<PedidoResumenViewModel>();

        public static List<string> FormasPagoDisponibles =>
            Enum.GetNames(typeof(FormaPagoCliente)).ToList();

        // Conversión Model -> ViewModel
        public static ClienteViewModel FromCliente(Cliente cliente)
        {
            return new ClienteViewModel
            {
                Email = cliente.Email,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                DireccionesString = string.Join(", ", cliente.Direcciones),
                TelefonosString = string.Join(", ", cliente.Telefonos),
                EmailsString = string.Join(", ", cliente.Emails),
                AlergiasString = string.Join(", ", cliente.Alergias),
                FormaPago = cliente.FormaPago.ToString(),
                PuntosAcumulados = cliente.PuntosAcumulados,
                PuntosCanjeados = cliente.PuntosCanjeados,
                HistorialPedidosIds = "", // Lo dejamos limpio por ahora

                // CORRECCIÓN IMPORTANTE: Dejamos la lista vacía aquí.
                // La llenaremos en la página principal cruzando los datos.
                HistorialPedidos = new ObservableCollection<PedidoResumenViewModel>()
            };
        }

        public Cliente ToCliente()
        {
            return new Cliente
            {
                Email = this.Email,
                Nombre = this.Nombre,
                Apellidos = this.Apellidos,
                Direcciones = this.DireccionesString?.Split(',').Select(d => d.Trim()).Where(d => !string.IsNullOrEmpty(d)).ToList() ?? new List<string>(),
                Telefonos = this.TelefonosString?.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList() ?? new List<string>(),
                Emails = this.EmailsString?.Split(',').Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>(),
                Alergias = this.AlergiasString?.Split(',').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToList() ?? new List<string>(),
                FormaPago = (FormaPagoCliente)Enum.Parse(typeof(FormaPagoCliente), this.FormaPago),
                PuntosAcumulados = this.PuntosAcumulados,
                PuntosCanjeados = this.PuntosCanjeados,
                HistorialPedidos = new List<Pedido>() // Se queda vacío al guardar para no duplicar datos
            };
        }
    }
}