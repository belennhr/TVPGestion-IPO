using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class ClienteService
    {
        private readonly DataPersistenceService<ClienteViewModel> persistenceService;

        public ClienteService()
        {
            persistenceService = new DataPersistenceService<ClienteViewModel>("clientes.txt");
        }

        public List<ClienteViewModel> CargarClientes()
        {
            return persistenceService.LoadData();
        }

        public void GuardarClientes(List<ClienteViewModel> clientes)
        {
            persistenceService.SaveData(clientes);
        }

        public void AgregarCliente(ClienteViewModel clienteVM)
        {
            persistenceService.AddItem(clienteVM);
        }

        public void ActualizarCliente(string email, ClienteViewModel clienteActualizado)
        {
            persistenceService.UpdateItem(
                c => c.Email == email,
                clienteActualizado
            );
        }

        public void EliminarCliente(string email)
        {
            persistenceService.DeleteItem(c => c.Email == email);
        }

        public ClienteViewModel ObtenerClientePorEmail(string email)
        {
            var clientes = CargarClientes();
            return clientes.FirstOrDefault(c => c.Email == email);
        }

        // Procesar puntos usando la lógica del modelo
        public void ProcesarPuntosPorPedido(string clienteEmail, decimal importePedido, bool acumularPuntos, bool canjearEnvioGratis)
        {
            var clienteVM = ObtenerClientePorEmail(clienteEmail);
            if (clienteVM == null) return;

            var cliente = clienteVM.ToCliente();

            // Lógica de negocio en el modelo
            if (canjearEnvioGratis)
            {
                cliente.CanjearPuntosEnvioGratis();
            }
            else if (acumularPuntos)
            {
                cliente.AgregarPuntosPorPedido(importePedido, true);
            }

            // Actualizar ViewModel con los cambios del modelo
            clienteVM.PuntosAcumulados = cliente.PuntosAcumulados;
            clienteVM.PuntosCanjeados = cliente.PuntosCanjeados;

            ActualizarCliente(clienteEmail, clienteVM);
        }

        // Agregar pedido al historial del cliente
        public void AgregarPedidoACliente(string clienteEmail, string pedidoId)
        {
            var clienteVM = ObtenerClientePorEmail(clienteEmail);
            if (clienteVM == null) return;

            if (string.IsNullOrEmpty(clienteVM.HistorialPedidosIds))
            {
                clienteVM.HistorialPedidosIds = pedidoId;
            }
            else
            {
                var pedidosExistentes = clienteVM.HistorialPedidosIds.Split(',').Select(p => p.Trim()).ToList();
                if (!pedidosExistentes.Contains(pedidoId))
                {
                    clienteVM.HistorialPedidosIds += ", " + pedidoId;
                }
            }

            ActualizarCliente(clienteEmail, clienteVM);
        }

        // Obtener pedidos de un cliente
        public List<string> ObtenerPedidosCliente(string clienteEmail)
        {
            var clienteVM = ObtenerClientePorEmail(clienteEmail);
            if (clienteVM == null || string.IsNullOrEmpty(clienteVM.HistorialPedidosIds))
                return new List<string>();

            return clienteVM.HistorialPedidosIds.Split(',').Select(p => p.Trim()).ToList();
        }
    }
}