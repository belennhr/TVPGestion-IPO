using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class PedidoService
    {
        private readonly DataPersistenceService<Pedido> persistenceService;
        private readonly ClienteService clienteService;
        private readonly ProductoService productoService;

        public PedidoService()
        {
            persistenceService = new DataPersistenceService<Pedido>("pedidos.txt");
            clienteService = new ClienteService();
            productoService = new ProductoService();
        }

        public List<PedidoViewModel> CargarPedidos()
        {
            var pedidosModel = persistenceService.LoadData();
            
            // Si no hay pedidos, inicializar con datos de ejemplo
            if (pedidosModel.Count == 0)
            {
                pedidosModel = InicializarPedidosEjemplo();
            }
            
            // Convertir Model ? ViewModel
            return pedidosModel.Select(p => PedidoViewModel.FromPedido(p)).ToList();
        }

        private List<Pedido> InicializarPedidosEjemplo()
        {
            var pedidosEjemplo = new List<Pedido>();
            var clientesVM = clienteService.CargarClientes();
            var productosVM = productoService.CargarProductos();

            // Verificar que existen clientes y productos
            if (clientesVM.Count == 0 || productosVM.Count == 0)
            {
                return pedidosEjemplo;
            }

            // Convertir productos a Model
            var productos = productosVM.Select(vm => vm.ToProducto()).ToList();

            // Crear pedido 1
            var pedido1 = new Pedido
            {
                Id = "PED001",
                FechaHoraRealizacion = DateTime.Now.AddDays(-2),
                Medio = MedioPedido.EnLocal,
                Modalidad = ModalidadEntrega.RecogerAhora,
                FechaHoraRecogida = null,
                ClienteEmail = clientesVM[0].EmailsString,
                FormaPago = clientesVM[0].FormaPago,
                Estado = EstadoPedido.Recogido,
                DireccionEntrega = "",
                CosteEnvio = 0,
                EnvioGratisCanjeado = false,
                AcumularPuntos = true
            };

            if (productos.Count > 0)
            {
                pedido1.Productos[productos[0]] = 2;
            }
            if (productos.Count > 1)
            {
                pedido1.Productos[productos[1]] = 1;
            }

            pedidosEjemplo.Add(pedido1);

            // Crear pedido 2 si hay más clientes
            if (clientesVM.Count > 1 && productos.Count > 0)
            {
                var pedido2 = new Pedido
                {
                    Id = "PED002",
                    FechaHoraRealizacion = DateTime.Now.AddDays(-1),
                    Medio = MedioPedido.Telefono,
                    Modalidad = ModalidadEntrega.Domicilio,
                    FechaHoraRecogida = DateTime.Now.AddHours(1),
                    ClienteEmail = clientesVM[1].EmailsString,
                    FormaPago = clientesVM[1].FormaPago,
                    Estado = EstadoPedido.EnElaboracion,
                    DireccionEntrega = clientesVM[1].DireccionesString.Split(',')[0].Trim(),
                    CosteEnvio = 3.50m,
                    EnvioGratisCanjeado = false,
                    AcumularPuntos = true
                };

                pedido2.Productos[productos[0]] = 1;

                pedidosEjemplo.Add(pedido2);
            }

            persistenceService.SaveData(pedidosEjemplo);
            return pedidosEjemplo;
        }

        public void GuardarPedidos(List<PedidoViewModel> pedidosVM)
        {
            var productosModel = productoService.CargarProductos().Select(vm => vm.ToProducto()).ToList();
            var pedidosModel = pedidosVM.Select(vm => vm.ToPedido(productosModel)).ToList();
            persistenceService.SaveData(pedidosModel);
        }

        public void AgregarPedido(PedidoViewModel pedidoVM)
        {
            var productosModel = productoService.CargarProductos().Select(vm => vm.ToProducto()).ToList();
            var pedido = pedidoVM.ToPedido(productosModel);
            persistenceService.AddItem(pedido);
        }

        public void ActualizarPedido(string id, PedidoViewModel pedidoVM)
        {
            var productosModel = productoService.CargarProductos().Select(vm => vm.ToProducto()).ToList();
            var pedidoActualizado = pedidoVM.ToPedido(productosModel);
            persistenceService.UpdateItem(
                p => p.Id == id,
                pedidoActualizado
            );
        }

        public void EliminarPedido(string id)
        {
            persistenceService.DeleteItem(p => p.Id == id);
        }

        public string GenerarNuevoId()
        {
            var pedidos = persistenceService.LoadData();
            if (pedidos.Count == 0)
            {
                return "PED001";
            }

            var maxId = pedidos
                .Select(p => p.Id)
                .Where(id => id.StartsWith("PED"))
                .Select(id => int.TryParse(id.Substring(3), out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"PED{(maxId + 1):D3}";
        }
    }
}