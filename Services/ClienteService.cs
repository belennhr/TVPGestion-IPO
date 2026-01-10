using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class ClienteService
    {
        private readonly DataPersistenceService<Cliente> persistenceService;

        public ClienteService()
        {
            persistenceService = new DataPersistenceService<Cliente>("clientes.txt");
        }

        public List<ClienteViewModel> CargarClientes()
        {
            var clientesModel = persistenceService.LoadData();
            
            // Si no hay datos, inicializar con ejemplos
            if (clientesModel.Count == 0)
            {
                InicializarClientesEjemplo();
                clientesModel = persistenceService.LoadData();
            }
            
            // Convertir Model a ViewModel
            return clientesModel.Select(c => ClienteViewModel.FromCliente(c)).ToList();
        }

        public void GuardarClientes(List<ClienteViewModel> clientesVM)
        {
            // Convertir ViewModel a Model
            var clientesModel = clientesVM.Select(vm => vm.ToCliente()).ToList();
            persistenceService.SaveData(clientesModel);
        }

        public void InicializarClientesEjemplo()
        {
            var clientesEjemplo = new List<Cliente>
            {
                new Cliente 
                { 
                    Email = "juan@mail.com",
                    Nombre = "Juan", 
                    Apellidos = "Perez", 
                    Direcciones = new List<string> { "Calle Mayor 1, Madrid" },
                    Telefonos = new List<string> { "123456789" },
                    Emails = new List<string> { "juan@mail.com" },
                    Alergias = new List<string> { "Ninguna" },
                    FormaPago = FormaPagoCliente.Tarjeta, 
                    PuntosAcumulados = 100 
                },
                new Cliente 
                { 
                    Email = "ana@mail.com",
                    Nombre = "Ana", 
                    Apellidos = "Garcia", 
                    Direcciones = new List<string> { "Avenida Central 2, Barcelona" },
                    Telefonos = new List<string> { "987654321" },
                    Emails = new List<string> { "ana@mail.com" },
                    Alergias = new List<string> { "Gluten" },
                    FormaPago = FormaPagoCliente.Bizum, 
                    PuntosAcumulados = 50 
                }
            };

            persistenceService.SaveData(clientesEjemplo);
        }

        public void AgregarCliente(ClienteViewModel clienteVM)
        {
            var cliente = clienteVM.ToCliente();
            persistenceService.AddItem(cliente);
        }

        public void ActualizarCliente(string email, ClienteViewModel clienteVM)
        {
            var clienteActualizado = clienteVM.ToCliente();
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

        // Procesar puntos usando la logica del modelo
        public void ProcesarPuntosPorPedido(string clienteEmail, decimal importePedido, bool acumularPuntos, bool canjearEnvioGratis)
        {
            var clienteVM = ObtenerClientePorEmail(clienteEmail);
            if (clienteVM == null) return;

            var cliente = clienteVM.ToCliente();

            // Logica de negocio en el modelo
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