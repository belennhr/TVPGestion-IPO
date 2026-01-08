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

        public void AgregarCliente(ClienteViewModel cliente)
        {
            persistenceService.AddItem(cliente);
        }

        public void ActualizarCliente(string nombre, string apellidos, ClienteViewModel clienteActualizado)
        {
            persistenceService.UpdateItem(
                c => c.Nombre == nombre && c.Apellidos == apellidos,
                clienteActualizado
            );
        }

        public void EliminarCliente(string nombre, string apellidos)
        {
            persistenceService.DeleteItem(c => c.Nombre == nombre && c.Apellidos == apellidos);
        }
    }
}